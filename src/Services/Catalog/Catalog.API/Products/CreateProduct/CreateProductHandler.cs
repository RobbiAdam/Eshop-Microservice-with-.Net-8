using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name, string Description, List<string> Categories, string ImageFile, decimal Price)
        : ICommand<CreateProductResponse>;


    public record CreateProductResponse(
        Guid Id);

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Categories = command.Categories,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            return new CreateProductResponse(product.Id);
            
        }
    }
}
