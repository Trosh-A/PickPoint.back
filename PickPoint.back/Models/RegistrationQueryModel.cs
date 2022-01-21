#nullable disable
namespace PickPoint.back.Models;

//EXPLAIN Сейчас LoginQueryModel повторяет RegistrationQueryModel, но в будущем при регистрации может понадобитсья больше данных
public class RegistrationQueryModel
{
  public string INN { get; set; }
  public string Password { get; set; }
}
