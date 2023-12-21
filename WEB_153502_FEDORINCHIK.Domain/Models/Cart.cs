using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_153502_FEDORINCHIK.Domain.Entities;

namespace WEB_153502_FEDORINCHIK.Domain.Models
{
    public class Cart
    {
        /// <summary>
        /// Список объектов в корзине
        /// key - идентификатор объекта
        /// </summary>
        public Dictionary<int, CartItem> CartItems { get; set; } = new();


        /// <summary>
        /// Добавить объект в корзину
        /// </summary>
        /// <param name="dish">Добавляемый объект</param>
        public virtual void AddToCart(Game game)
        {
            if (CartItems.ContainsKey(game.Id))
            {
                CartItems[game.Id].Count++;
            }
            else
            {
                CartItems[game.Id] = new CartItem()
                {
                    Game = game,
                    Count = 1
                };
            }
        }


        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id"> id удаляемого объекта</param>
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }


        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }


        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count
        {
            get => CartItems.Sum(item => item.Value.Count);
        }


        /// <summary>
        /// Общее количество калорий
        /// </summary>
        public double TotalPrice
        {
            get => (double)CartItems.Sum(item => item.Value.Game.Price * item.Value.Count);
        }
    }
}
