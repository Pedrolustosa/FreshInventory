using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Suppliers.Command.UpdateSupplier
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
    {
        private readonly ISupplierRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSupplierCommandHandler(ISupplierRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var existingSupplier = await _repository.GetByIdAsync(request.Id)
                ?? throw new ServiceException($"Supplier with ID {request.Id} not found.");

            _mapper.Map(request, existingSupplier);
            await _repository.UpdateAsync(existingSupplier);
        }
    }
}
