﻿using System.Threading.Tasks;
using TheLizzards.Common.Data;

namespace TheLizzards.CQRS
{
	public interface IDataWriter<T>
		where T : IAggregateRoot
	{
		Task Insert(T item);

		Task Update(T item);
	}
}