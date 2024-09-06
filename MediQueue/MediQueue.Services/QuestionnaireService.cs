using AutoMapper;
using MediQueue.Domain.DTOs.Questionnaire;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IMapper _mapper;

        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository, IMapper mapper)
        {
            _questionnaireRepository = questionnaireRepository ?? throw new ArgumentNullException(nameof(questionnaireRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<QuestionnaireDto>> GetAllQuestionnairesAsync()
        {
            var quest = await _questionnaireRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<QuestionnaireDto>>(quest);
        }

        public async Task<QuestionnaireDto> GetQuestionnaireByIdAsync(int id)
        {
            var quest = await _questionnaireRepository.FindByIdAsync(id);
            if (quest == null)
            {
                throw new KeyNotFoundException($"Questionnaire with {id} not found");
            }

            return _mapper.Map<QuestionnaireDto>(quest);
        }

        public async Task<QuestionnaireDto> CreateQuestionnaireAsync(QuestionnaireForCreateDto questionnaireForCreateDto)
        {
            if (questionnaireForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(questionnaireForCreateDto));
            }

            var quest = _mapper.Map<Questionnaire>(questionnaireForCreateDto);

            await _questionnaireRepository.CreateAsync(quest);

            return _mapper.Map<QuestionnaireDto>(quest);
        }

        public async Task<QuestionnaireDto> UpdateQuestionnaireAsync(QuestionnaireForUpdateDto questionnaireForUpdateDto)
        {
            if (questionnaireForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(questionnaireForUpdateDto));
            }

            var quest = _mapper.Map<Questionnaire>(questionnaireForUpdateDto);

            await _questionnaireRepository.UpdateAsync(quest);

            return _mapper.Map<QuestionnaireDto>(quest);
        }

        public async Task DeleteQuestionnaireAsync(int id)
        {
            await _questionnaireRepository.DeleteAsync(id);
        }
    }
}
