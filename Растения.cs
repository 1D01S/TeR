//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeR
{
    using System;
    using System.Collections.Generic;
    
    public partial class Растения
    {
        public int plant_id { get; set; }
        public string название { get; set; }
        public string тип { get; set; }
        public string описание { get; set; }
        public Nullable<int> biome_id { get; set; }
        public Nullable<int> update_id { get; set; }
    
        public virtual Биомы Биомы { get; set; }
        public virtual История_Обновлений История_Обновлений { get; set; }
    }
}
