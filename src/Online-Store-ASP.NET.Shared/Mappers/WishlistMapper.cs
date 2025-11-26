using System.Linq;
using System.Collections.Generic;
using Shared.DTO.User;
using Shared.DTO.Wishlist;
using Shared.DTO.WishlistItem;
using Shared.DTO.Product;
using Shared.DTO.Review;
using Shared.Models;

namespace Shared.Mappers
{
    public static class WishlistMapper
    {
        public static WishlistReadDto ToReadDto(Wishlist w)
        {
            return new WishlistReadDto
            {
                Id = w.Id,
                UserId = w.UserId,
                ItemsCount = w.WishlistItems?.Count ?? 0,
                WishlistItems = w.WishlistItems?.Select(i => new WishlistItemReadDto
                {
                    Id = i.Id,
                    WishlistId = i.WishlistId,
                    ProductId = i.ProductId,
                    Product = i.Product != null ? new ProductReadDto
                    {
                        Id = i.Product.Id,
                        Article = i.Product.Article,
                        Name = i.Product.Name,
                        Description = i.Product.Description,
                        Price = i.Product.Price,
                        MediaUrls = i.Product.MediaUrls ?? new List<string>(),
                        Categories = new List<ProductCategoryDto>()
                    } : null!
                }).ToList() ?? new List<WishlistItemReadDto>()
            };
        }

        public static WishlistShortDto ToShortDto(Wishlist w)
        {
            return new WishlistShortDto
            {
                Id = w.Id,
                UserId = w.UserId
            };
        }

        public static WishlistDetailedDto ToDetailedDto(Wishlist w)
        {
            return new WishlistDetailedDto
            {
                Id = w.Id,
                UserId = w.UserId,
                User = w.User != null
                    ? new UserDetailedDto
                    {
                        Id = w.User.Id,
                        Username = w.User.Username,
                        Name = w.User.Name,
                        Email = w.User.Email,
                        Phone = w.User.Phone,
                        Role = w.User.Role.ToString()
                    }
                    : null!,
                WishlistItems = w.WishlistItems?.Select(i => new WishlistItemDetailedDto
                {
                    Id = i.Id,
                    WishlistId = i.WishlistId,
                    ProductId = i.ProductId,
                    Product = i.Product != null ? new ProductDetailsDto
                    {
                        Id = i.Product.Id,
                        Article = i.Product.Article,
                        Name = i.Product.Name,
                        Description = i.Product.Description,
                        Price = i.Product.Price,
                        MediaUrls = i.Product.MediaUrls ?? new List<string>(),
                        Categories = i.Product.Categories?.Select(c => new ProductCategoryDto
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList() ?? new List<ProductCategoryDto>(),
                        Reviews = new List<ProductReviewShortDto>(),
                        AverageRating = 0
                    } : null!
                }).ToList() ?? new List<WishlistItemDetailedDto>(),
                Items = w.WishlistItems?.Select(i => new WishlistItemDto
                {
                    Id = i.Id,
                    WishlistId = i.WishlistId,
                    ProductId = i.ProductId,
                    AddedAt = i.AddedAt
                }).ToList() ?? new List<WishlistItemDto>()
            };
        }
    }
}