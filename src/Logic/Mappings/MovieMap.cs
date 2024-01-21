﻿using FluentNHibernate;
using FluentNHibernate.Mapping;
using Logic.Entities;

namespace Logic.Mappings;

public class MovieMap : ClassMap<Movie>
{
    public MovieMap()
    {
        Id(x => x.Id);

        DiscriminateSubClassesOnColumn("LicensingModel");

        Map(x => x.Name);
        Map(Reveal.Member<Movie>("LicensingModel")).CustomType<int>();
    }

    public class TwoDaysMovieMap : SubclassMap<TwoDaysMovie>
    {
        public TwoDaysMovieMap()
        {
            DiscriminatorValue((int)LicensingModel.TwoDays);
        }
    }

    public class LifeLongMovieMap : SubclassMap<LifeLongMovie>
    {
        public LifeLongMovieMap()
        {
            DiscriminatorValue((int)LicensingModel.LifeLong);
        }
    }
}
