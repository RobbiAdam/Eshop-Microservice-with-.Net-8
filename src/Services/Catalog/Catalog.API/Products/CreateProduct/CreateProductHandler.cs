namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name, string Description, List<string> Category, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(
        Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category cannot be empty");
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductCommandHandler(
        IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Category = command.Category,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
            
        }
    }
}
