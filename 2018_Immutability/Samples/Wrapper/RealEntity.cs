using ImmutableNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Wrapper
{
    class RealEntity
    {
        public RealEntity(int id, Credentials<RealEntity> credentials, Address address, BirthInformation birthInformation)
        {
            Id = id;
            Credentials = Immutable.Create(credentials);
            BirthInformation = Immutable.Create(birthInformation);
            Address = Immutable.Create(address);
        }
        public int Id { get; }
        public Immutable<Credentials<RealEntity>> Credentials { get; }
        public Immutable<Address> Address { get; }
        public Immutable<BirthInformation> BirthInformation { get; }
    }

    class Credentials<T>
    {
        /*---*/
    }

    class Address
    {
        /*---*/
    }

    class BirthInformation
    {
        /*---*/
    }

}
