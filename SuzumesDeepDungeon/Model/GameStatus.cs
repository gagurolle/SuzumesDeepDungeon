using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SuzumesDeepDungeon.Model
{
    public enum GameStatus
    {
        [Display(Name = "Пройдено")]
        Completed = 0,

        [Display(Name = "В процессе")]
        InProgress = 1,

        [Display(Name = "Дроп")]
        Drop = 2,

        [Display(Name = "Отложено")]
        OnHold = 3,

        [Display(Name = "Планирую играть")]
        PlantoPlay = 4,

       [Display(Name = "Сетевая игра")]
        NetworkGame = 5,

        [Display(Name = "Неизвестно")]
        Unknown = 6
    }
}
