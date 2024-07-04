using Catalog.API.Exceptions;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(
        Guid Id, string Name, List<String> Category, 
        string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id cannot be empty");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price cannot be empty and must be greater than 0");
        }
    }

    internal class UpdateProductCommandHandler(
        IDocumentSession session) 
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(
            UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

            if (product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;
            
            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }
}
