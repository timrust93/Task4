using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.EJ2.Base;
using System.Collections;
using Task4.Services;
using Task4.Db;
using Task4.AuthorizationHelpers;
using Task4.Library;


namespace Task4.Pages
{
    [Authorize(Policy = AppPolicies.ADMIN_POLICY)]    
    public class AdminPanelModel : PageModel
    {
        private sealed class UserDisplay
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public DateTime LastSeen { get; set; }
            public string LastSeenStr { get; set; } = string.Empty;
            public bool IsBlocked { get; set; }
        }

        public enum UserModifyAction
        {
            Block,
            Unblock,
            Delete
        }


        private readonly AppDbContext _dbContext;
        private readonly RegistrationServices _registrationServices;        

        public AdminPanelModel(AppDbContext dbContext, RegistrationServices registrationServices)
        {
            _dbContext = dbContext;            
            _registrationServices = registrationServices;
        }

        public IActionResult OnPostUrlDataSourceAjax([FromBody] DataManagerRequest dm)
        {
            List<UserDisplay> userDisplayList = GetAllRecords();
            userDisplayList.Sort((x, y) => y.LastSeen.CompareTo(x.LastSeen));
            IEnumerable DataSource = userDisplayList.AsEnumerable();
            DataOperations operation = new DataOperations();
                      

            if (dm.Select != null)
            {
                DataSource = operation.PerformSelect(DataSource, dm.Select);
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }

            int count = userDisplayList.Count;            

            return dm.RequiresCounts ?
                new JsonResult(new { result = DataSource, count = count }) : new JsonResult(DataSource);
        }

        public async Task<JsonResult> OnPostActOnRecords([FromBody] List<int> ids, [FromQuery] UserModifyAction userModifyAction)
        {
            if (!IsUserEligible(out string whyNot))
            {
                await HttpContext.SignOutAsync(AuthHelper.AUTH_COOKIE);
                return RedirectToLoginAjax(whyNot);
            }

            string webClientEmail = AuthHelper.GetWebClientEmail(User);
            string selfActionMessage = "";

            foreach (var id in ids)
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
                if (user == null) continue;
                if (userModifyAction == UserModifyAction.Block)
                    user.IsBlocked = true;
                else if (userModifyAction == UserModifyAction.Unblock)
                    user.IsBlocked = false;
                else if (userModifyAction == UserModifyAction.Delete)
                    _dbContext.Users.Remove(user);
                if (user.Email == webClientEmail)
                    selfActionMessage = GetSelfActionMessage();
            }
            await _dbContext.SaveChangesAsync();

            if (string.IsNullOrEmpty(selfActionMessage))
            {
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            else
            {
                await HttpContext.SignOutAsync(AuthHelper.AUTH_COOKIE);
                return RedirectToLoginAjax(selfActionMessage);
            }            

            string GetSelfActionMessage()
            {
                string message = "Action successful";
                if (userModifyAction == UserModifyAction.Block)
                    return message + " And you blocked yourself";
                else if (userModifyAction == UserModifyAction.Delete)
                    return message + " And you deleted yourself";
                else
                    return string.Empty;
            }
            
        }
        

        private bool IsUserEligible(out string whyNot)
        {            
            whyNot = string.Empty;
            string webClientEmail = AuthHelper.GetWebClientEmail(User);
            User? user = _registrationServices.FindRegisteredUser(webClientEmail);
            if (user == null)
            {
                whyNot = "Your account was deleted by someone";
                return false;
            }
            else if (user.IsBlocked)
            {
                whyNot = "Your account was blocked by someone";
                return false;
            }
            return true;            
        }

        private JsonResult RedirectToLoginAjax(string reason)
        {
            return new JsonResult(new { redirectUrl = "/Login", reason = reason });
        }

        private List<UserDisplay> GetAllRecords()
        {
            List<UserDisplay> list = _dbContext.Users.Select(x =>            
                new UserDisplay()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    LastSeen = x.LastSeen,
                    LastSeenStr = LastSeenGetter.GetLastSeenTime(x.LastSeen),
                    IsBlocked = x.IsBlocked
                }).ToList();
            return list;
        }
    }
}
