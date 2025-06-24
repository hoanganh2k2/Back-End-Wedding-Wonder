using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;
using WeddingWonderAPI.Models.DTOs;

namespace Services
{
    public class EventConceptServiceManager
    {
        private readonly IEventConceptRepository _eventConceptRepository;

        public EventConceptServiceManager(IEventConceptRepository eventConceptRepository)
        {
            _eventConceptRepository = eventConceptRepository;
        }

        public async Task<List<EventConceptDTO>> GetAllEventConceptsAsync()
        {
            try
            {
                List<EventConcept> concepts = await _eventConceptRepository.GetsAsync();
                return concepts.Select(c => new EventConceptDTO
                {
                    ConceptId = c.ConceptId,
                    ConceptName = c.ConceptName,
                    PackageId = c.PackageId,
                    Status = c.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<EventConceptDTO?> GetEventConceptByIdAsync(int id)
        {
            try
            {
                EventConcept concept = await _eventConceptRepository.GetAsyncById(id);
                if (concept == null) return null;

                return new EventConceptDTO
                {
                    ConceptId = concept.ConceptId,
                    ConceptName = concept.ConceptName,
                    PackageId = concept.PackageId,
                    Status = concept.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<EventConceptDTO>> GetEventConceptsByPackageIdAsync(int packageId)
        {
            try
            {
                List<EventConcept> concepts = await _eventConceptRepository.GetEventConceptsByPackageId(packageId);

                return concepts.Select(c => new EventConceptDTO
                {
                    ConceptId = c.ConceptId,
                    ConceptName = c.ConceptName,
                    PackageId = c.PackageId,
                    Status = c.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateEventConceptAsync(EventConceptDTO conceptDto)
        {
            try
            {
                EventConcept concept = new()
                {
                    ConceptName = conceptDto.ConceptName,
                    PackageId = conceptDto.PackageId,
                    Status = conceptDto.Status
                };
                return await _eventConceptRepository.CreateAsync(concept);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateEventConceptAsync(int id, EventConceptDTO conceptDto)
        {
            try
            {
                EventConcept existingConcept = await _eventConceptRepository.GetAsyncById(id);
                if (existingConcept == null) return false;

                existingConcept.ConceptName = conceptDto.ConceptName;
                existingConcept.PackageId = conceptDto.PackageId;
                existingConcept.Status = conceptDto.Status;

                await _eventConceptRepository.UpdateAsync(id, existingConcept);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteEventConceptAsync(int id)
        {
            try
            {
                await _eventConceptRepository.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
