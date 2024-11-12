using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Application.IRepository
{
	public interface ICardRepository
	{
		Task<IEnumerable<Card>> GetAllCardsByUserIdAsync(string UserId);
		Task AddCardAsync(Card card);
	}
}
