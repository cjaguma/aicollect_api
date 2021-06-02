
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiCollect.Core
{
    public class AICollect:AiCollectObject
    {
        public AICollect(AiCollectObject parent) : base(parent)
        {
        }

        #region Events
        public event SessionAuthenticationFailedHandler AuthenticationSucceeded;
        public event SessionAuthenticationFailedHandler AuthenticationFailed;
        public event SessionProjectsLoadSuccessfulHandler ProjectsLoadSucceeded;
        public event SessionProjectsLoadFailedHandler ProjectsLoadFailed;
        #endregion


        public string Token { get; set; }
        public User User { get; set; }
        public Configuration ActiveProject { get; set; }
        public IEnumerable<Configuration> Projects { get; private set; }
        public IEnumerable<string> Roles { get; set; }

        public UserAccountSummary AccountSummary { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; protected set; }

       
        //public Session(string username, string password)
        //{
        //    User = CreateUser(username, password);
        //}

        //public Session(User user)
        //{
        //    User = user;
        //}

        private void OnAuthenticationSucceeded()
        {
            if (AuthenticationSucceeded != null)
                AuthenticationSucceeded();
        }

        private void OnAuthenticationFailed()
        {
            if (AuthenticationFailed != null)
                AuthenticationFailed();
        }

        private void OnProjectsLoadSucceeded()
        {
            if (ProjectsLoadSucceeded != null)
                ProjectsLoadSucceeded();
        }

        private void OnProjectsLoadFailed()
        {
            if (ProjectsLoadFailed != null)
                ProjectsLoadFailed();
        }

        public void KeepAlive()
        {


        }

        public virtual bool Authenticate(bool preLoadProjects = true)
        {
            //if (IsAuthenticated)
            //    return true;

            //AuthResponse response = null;
            //try
            //{
            //    response = User.Login().Result;

            //    if (response != null && response.Status == ResponseStatuses.Success)
            //    {
            //        Token = response.Token;
            //        User.SetKey(Token);
            //        IsAuthenticated = true;

            //        OnAuthenticationSucceeded();
            //        //Load User stats
            //        RefreshAccountSummary();

            //        //Load projects
            //        if (preLoadProjects)
            //        {
            //            RefreshUserProjects();
            //        }

            //        return true;
            //    }
            //    else
            //    {
            //        throw new Exception("Authentication failed");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    IsAuthenticated = false;
            //    OnAuthenticationFailed();
            //    throw ex;
            //}

            return false;
        }

        public virtual bool Authenticate(string username, string password, bool preLoadProjects = true)
        {
            User = CreateUser(username, password);

            return Authenticate(preLoadProjects);
        }

        private User CreateUser(string username, string password)
        {
            User u = new User(null);
            u.UserName = username;
            u.Password = password;

            return u;
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        //public void RefreshUserProjects()
        //{
        //    if (!IsAuthenticated)
        //        throw new Exception("User not authenticated");

        //    try
        //    {
        //        DataProvider provider = new DataProvider();
        //        Projects = provider.Download<IEnumerable<Configuration>>(string.Format("/Project/Find/?key={0}", Token), User.UserName, User.Password).Result;

        //        OnProjectsLoadSucceeded();
        //    }
        //    catch
        //    {
        //        OnProjectsLoadFailed();
        //    }
        //}

        //public void RemoveUserProjects(string configKey)
        //{
        //   //still testing it at the ui
        //}

        //public void RefreshAccountSummary()
        //{
        //    if (!IsAuthenticated)
        //        throw new Exception("User not authenticated");

        //    try
        //    {
        //        DataProvider provider = new DataProvider();
        //        AccountSummary = provider.Download<UserAccountSummary>(string.Format("/Subscription/Discover/?key={0}", Token), User.UserName, User.Password).Result;


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }

    public delegate void SessionAuthenticationSuccessHandler();
    public delegate void SessionAuthenticationFailedHandler();
    public delegate void SessionProjectsLoadSuccessfulHandler();
    public delegate void SessionProjectsLoadFailedHandler();
    public delegate void SessionAccountSummaryLoadFailedHandler();
    public delegate void SessionAccountSummaryLoadSuccessfulHandler();

}
