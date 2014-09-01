﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KBVault.Web.Models.Public
{
    public class KbSearchResultItemViewModel
    {
        public bool IsArticle { get; set; }
        public bool IsAttachment { get; set; }
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleSefName { 
            get{
                using (var db = new KBVault.Dal.KbVaultEntities())
                {
                    var article = db.Articles.FirstOrDefault(a => a.Id == ArticleId);
                    if (article != null)
                        return article.SefName;
                    return String.Empty;
                }
            }
        }
    }
}