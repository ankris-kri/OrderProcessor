using System;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessor.Models
{
    public class BasketItem
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public String ProductCode { get; set; }
        [Required]
        public BasketAction Action { get; set; }
        [Required]
        public ProductCategoryName? ProductCategoryName { get; set; }
        [Required]
        public ProductGroupName? ProductGroupName { get; set; }
    }

    public enum BasketAction
    {
        Add,
        Update
    }

    public enum ProductGroupName
    {
        Book,
        Membership,
        Video
    }

    public enum ProductCategoryName
    {
        Physical,
        Virtual
    }
}
