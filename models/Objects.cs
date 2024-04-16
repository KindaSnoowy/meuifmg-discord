using System;
using System.Collections.Generic;

public class Etapa {
    public string Nome {get; set;}
    public List<Matéria> Matérias {get; set;}
}

public class Matéria {
    public string Nome {get; set;}
    public string Situação {get; set;}
    public List<Avaliação> Avaliações {get; set;} 
    public float SomatórioTotal {get; set;}
    public float SomatórioObtido {get; set;}
}

public class Avaliação {
    public string Nome {get; set;}
    public string Data {get; set;}
    public string DataDevolução {get; set;}
    public float Valor {get; set;}
    public float Nota {get; set;}   
}

