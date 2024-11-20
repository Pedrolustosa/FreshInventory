using MediatR;
using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.CQRS.Suppliers.Command.CreateSupplier
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, SupplierDto>
    {
        private readonly ISupplierRepository _repository;
        private readonly IMapper _mapper;

        public CreateSupplierCommandHandler(ISupplierRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SupplierDto> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = _mapper.Map<Supplier>(request);
            await _repository.AddAsync(supplier);

            return _mapper.Map<SupplierDto>(supplier);
        }
    }
}
