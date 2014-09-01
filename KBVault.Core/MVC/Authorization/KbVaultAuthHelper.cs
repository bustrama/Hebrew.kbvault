﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KBVault.Dal;
using KBVault.Core.Exceptions;
using KBVault.Core.Outlib;
using NLog;

namespace KBVault.Core.MVC.Authorization
{
    public class KbVaultAuthHelper
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static string HashAlgoritm = "SHA1";

        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_MANAGER = "Manager";
        public static string ROLE_EDITOR = "Editor";

        public static KbUser GetKbUser(string userName)
        {
            using (KbVaultEntities db = new KbVaultEntities())
            {
                return db.KbUsers.FirstOrDefault<KbUser>(ku => ku.UserName == userName);
            }

        }

        public static KbUser CreateUser(string username, string password, string email,string role)
        {
            try
            {
                using (KbVaultEntities db = new KbVaultEntities())
                {
                    KbUser usr = new KbUser();
                    usr.Password = HashPassword(password, Guid.NewGuid().ToString().Replace("-", ""));
                    usr.UserName = username;
                    usr.Email = email;
                    usr.Role = role;
                    db.KbUsers.Add(usr);
                    db.SaveChanges();
                    return usr;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public static string HashPassword(string password, string salt)
        {
            try
            {
                return ObviexSimpleHash.ComputeHash(password, HashAlgoritm, Encoding.Default.GetBytes(salt));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        
        private static bool VerifyHash(string password, string passwordHash) 
        { 
            return ObviexSimpleHash.VerifyHash(password, HashAlgoritm, passwordHash); 
        }

        public static bool ValidateUser(string userName, string password) 
        { 
            try 
            {
                using (var db = new KbVaultEntities())
                {
                    KbUser usr = GetKbUser(userName);
                    if (usr == null)
                        return false;
                    return VerifyHash(password, usr.Password); 
                }
                
            } 
            catch (Exception ex) 
            { 
                Log.Error(ex); 
                throw; 
            } 
        }

        public static void ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                if (ValidateUser(username, oldPassword))
                {
                    using (var db = new KbVaultEntities())
                    {
                        KbUser usr= db.KbUsers.FirstOrDefault(ku => ku.UserName == username);
                        if (usr != null)
                        {
                            usr.Password = HashPassword(newPassword, Guid.NewGuid().ToString().Replace("-", ""));
                            db.SaveChanges();
                        }
                        else throw new UserNotFoundException();
                    }                   
                }
                else
                {
                    throw new InvalidPasswordException();
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
    }
}
