using MvcAdvertizer.Core.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAdvertizer.Core.ViewModels
{
    public class SortViewModel
    {
        public AdvertSortState NumberSort { get; set; } // значение для сортировки по имени
        public AdvertSortState CreatedOnSort { get; set; }    // значение для сортировки по возрасту
        public AdvertSortState RateSort { get; set; }   // значение для сортировки по компании
        public AdvertSortState Current { get; set; }     // значение свойства, выбранного для сортировки
        public bool Up { get; set; }  // Сортировка по возрастанию или убыванию

        public SortViewModel(AdvertSortState sortOrder)
        {
            // значения по умолчанию
            NumberSort = AdvertSortState.NumberDesc;
            CreatedOnSort = AdvertSortState.CreatedOnDesc;
            RateSort = AdvertSortState.RateDesc;
            Up = true;

            if (sortOrder == AdvertSortState.NumberDesc || sortOrder == AdvertSortState.CreatedOnDesc
                || sortOrder == AdvertSortState.RateDesc)
            {
                Up = false;
            }

            switch (sortOrder)
            {
                case AdvertSortState.NumberDesc:
                    Current = NumberSort = AdvertSortState.NumberAsc;
                    break;
                case AdvertSortState.CreatedOnAsc:
                    Current = CreatedOnSort = AdvertSortState.CreatedOnDesc;
                    break;
                case AdvertSortState.CreatedOnDesc:
                    Current = CreatedOnSort = AdvertSortState.CreatedOnAsc;
                    break;
                case AdvertSortState.RateAsc:
                    Current = RateSort = AdvertSortState.RateDesc;
                    break;
                case AdvertSortState.RateDesc:
                    Current = RateSort = AdvertSortState.RateAsc;
                    break;
                default:
                    Current = NumberSort = AdvertSortState.NumberDesc;
                    break;
            }
        }
    }
}
