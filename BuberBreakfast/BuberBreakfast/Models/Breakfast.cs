namespace BuberBreakfast.Models;

public class Breakfast
    {
    public Breakfast(Guid id,
                     string name,
                     string description,
                     DateTime startDateTime,
                     DateTime endDateTime,
                     DateTime lastModifiedDateTime,
                     List<string> savory,
                     List<string> sweet)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }

    public static object MinNameLength { get; internal set; }
    public static object MaxNameLength { get; internal set; }
    public static object MinDescriptionLength { get; internal set; }
    public static object MaxDescriptionLength { get; internal set; }
    public Guid Id { get;  }    
    public string Name { get;  }
    public string  Description { get;  }
    public DateTime StartDateTime { get;  }
    public DateTime EndDateTime { get;  }
    public DateTime LastModifiedDateTime { get;  }
    public List<string> Savory { get;  }
    public List<string> Sweet { get;  }   

}