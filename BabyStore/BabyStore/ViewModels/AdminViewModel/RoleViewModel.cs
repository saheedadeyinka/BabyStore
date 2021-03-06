﻿using System.ComponentModel.DataAnnotations;

namespace BabyStore.ViewModels.AdminViewModel
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}