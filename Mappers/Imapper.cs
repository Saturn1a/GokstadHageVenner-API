namespace GokstadHageVennerAPI.Mappers;

public interface Imapper<TModel, TDto>
{
    TDto MapToDTO(TModel model);

    TModel MapToModel(TDto dto);


}
