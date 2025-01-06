using Microsoft.EntityFrameworkCore;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IRepositories;
using Travel.Infrastructure.Data;

namespace Travel.Infrastructure.Repositories
{
    public class StatiscalRepository(TravelDbContext dbContext) : IStatiscalRepository, IRepository
    {
        private readonly TravelDbContext _dbContext = dbContext;
        public async Task<StatisticalResponse> AdminStatiscal(StatisticalRequest request)
        {
            IQueryable<BookingRoom> bookingRoomQuery = null;
            IQueryable<BookingTour> bookingTourQuery = null;
            IQueryable<Payment> paymentQuery = null;

            //if (request.HotelId.HasValue)
            //{
            //    bookingRoomQuery = _dbContext.BookingRoom
            //        .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date)
            //        .Where(b => b.Room.HotelId == request.HotelId);

            //    paymentQuery = _dbContext.Payment
            //        .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date)
            //        .Where(p => p.BookingRoom.Room.HotelId == request.HotelId);
            //}

            //else if (request.TourId.HasValue)
            //{
            //    bookingTourQuery = _dbContext.BookingTour
            //        .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date)
            //        .Where(b => b.TourSchedule.TourId == request.TourId);

            //    paymentQuery = _dbContext.Payment
            //        .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date)
            //        .Where(p => p.BookingTour.TourSchedule.TourId == request.TourId);
            //}

            //else
            //{
            //    bookingRoomQuery = _dbContext.BookingRoom
            //        .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date);

            //    bookingTourQuery = _dbContext.BookingTour
            //        .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date);

            //    paymentQuery = _dbContext.Payment
            //        .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date);
            //}

            bookingRoomQuery = _dbContext.BookingRoom
                                .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date);

            bookingTourQuery = _dbContext.BookingTour
                                .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date);

            paymentQuery = _dbContext.Payment
                                .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date);

            var totalBookingRoom = bookingRoomQuery != null ? await bookingRoomQuery.CountAsync() : 0;
            var totalBookingTour = bookingTourQuery != null ? await bookingTourQuery.CountAsync() : 0;
            var TotalBooking = totalBookingRoom + totalBookingTour;

            var totalBookingRoomSuccess = bookingRoomQuery != null ? await bookingRoomQuery.Where(b => b.Status == 5).CountAsync() : 0;
            var totalBookingRoomPending = bookingRoomQuery != null ? await bookingRoomQuery.Where(b => b.Status == 0 || b.Status == 1).CountAsync() : 0;
            var totalBookingTourSuccess = bookingTourQuery != null ? await bookingTourQuery.Where(b => b.Status == 5).CountAsync() : 0;
            var totalBookingTourPending = bookingTourQuery != null ? await bookingTourQuery.Where(b => b.Status == 0 || b.Status == 1).CountAsync() : 0;

            var TotalBookingSuccess = totalBookingRoomSuccess + totalBookingTourSuccess;
            var TotalBookingPending = totalBookingRoomPending + totalBookingTourPending;
            var TotalBookingCancel = TotalBooking - TotalBookingSuccess - TotalBookingPending;

            var TotalPaymentAccessValue = paymentQuery != null ? await paymentQuery.Where(p => p.Type).SumAsync(p => p.Amount) : 0;
            var TotalPaymentRefundValue = paymentQuery != null ? await paymentQuery.Where(p => !p.Type).SumAsync(p => p.Amount) : 0;

            var statisticalDays = new List<StatisticalDay>();

            if (bookingRoomQuery != null)
            {
                var roomStatsByDay = await bookingRoomQuery
                    .GroupBy(b => b.CreatedAt.Date)
                    .Select(g => new StatisticalDay
                    {
                        Day = g.Key,
                        TotalBooking = g.Count(),
                        TotalBookingSuccess = g.Count(x => x.Status == 5),
                        TotalBookingPending = g.Count(x => x.Status == 0 || x.Status == 1),
                        TotalBookingCancel = g.Count(x => x.Status == 2 || x.Status == 3 || x.Status == 4)
                    })
                    .ToListAsync();

                statisticalDays.AddRange(roomStatsByDay);
            }

            if (bookingTourQuery != null)
            {
                var tourStatsByDay = await bookingTourQuery
                    .GroupBy(b => b.CreatedAt.Date)
                    .Select(g => new StatisticalDay
                    {
                        Day = g.Key,
                        TotalBooking = g.Count(),
                        TotalBookingSuccess = g.Count(x => x.Status == 5),
                        TotalBookingPending = g.Count(x => x.Status == 0 || x.Status == 1),
                        TotalBookingCancel = g.Count(x => x.Status == 2 || x.Status == 3 || x.Status == 4)
                    })
                    .ToListAsync();

                //statisticalDays.AddRange(tourStatsByDay);
                foreach (var tourStat in tourStatsByDay)
                {
                    var existingDay = statisticalDays.FirstOrDefault(s => s.Day == tourStat.Day);
                    if (existingDay != null)
                    {
                        existingDay.TotalBooking += tourStat.TotalBooking;
                        existingDay.TotalBookingSuccess += tourStat.TotalBookingSuccess;
                        existingDay.TotalBookingPending += tourStat.TotalBookingPending;
                        existingDay.TotalBookingCancel += tourStat.TotalBookingCancel;
                    }
                    else
                    {
                        statisticalDays.Add(tourStat);
                    }
                }
            }

            if (paymentQuery != null)
            {
                var paymentStatsByDay = await paymentQuery
                    .GroupBy(p => p.CreatedAt.Date)
                    .Select(g => new StatisticalDay
                    {
                        Day = g.Key,
                        PaymentValue = g.Sum(p => p.Type ? p.Amount : 0),
                        RefundValue = g.Sum(p => p.Type ? 0 : p.Amount)
                    })
                    .ToListAsync();

                foreach (var paymentStat in paymentStatsByDay)
                {
                    var existingDay = statisticalDays.FirstOrDefault(s => s.Day == paymentStat.Day);
                    if (existingDay != null)
                    {
                        existingDay.PaymentValue += paymentStat.PaymentValue;
                        existingDay.RefundValue += paymentStat.RefundValue;
                    }
                    else
                    {
                        statisticalDays.Add(paymentStat);
                    }
                }
            }

            return new StatisticalResponse
            {
                TotalBooking = TotalBooking,
                TotalBookingSuccess = TotalBookingSuccess,
                TotalBookingPending = TotalBookingPending,
                TotalBookingCancel = TotalBookingCancel,
                TotalPaymentAccessValue = TotalPaymentAccessValue,
                TotalPaymentRefundValue = TotalPaymentRefundValue,
                StatisticalDay = statisticalDays.OrderBy(s => s.Day)
            };
        }

        public async Task<StatisticalResponse> HotelStatiscal(StatisticalRequest request)
        {
            var bookingRoomQuery = _dbContext.BookingRoom
                    .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date && b.Room.Hotel.UserId == request.UserId)
                    .AsQueryable();

            var paymentQuery = _dbContext.Payment
                    .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date && p.BookingRoom.Room.Hotel.UserId == request.UserId)
                    .AsQueryable();

            if (request.HotelId.HasValue)
            {
                bookingRoomQuery = bookingRoomQuery.Where(b => b.Room.HotelId == request.HotelId);
                paymentQuery = paymentQuery.Where(p => p.BookingRoom.Room.HotelId == request.HotelId);
            }

            var TotalBooking = await bookingRoomQuery.CountAsync();
            var TotalBookingSuccess = await bookingRoomQuery.CountAsync(b => b.Status == 5);
            var TotalBookingPending = await bookingRoomQuery.CountAsync(b => b.Status == 0 || b.Status == 1);
            var TotalBookingCancel = TotalBooking - TotalBookingSuccess - TotalBookingPending;
            var TotalPaymentAccessValue = await paymentQuery.SumAsync(p => p.Type ? p.Amount : 0);
            var TotalPaymentRefundValue = await paymentQuery.SumAsync(p => p.Type ? 0 : p.Amount);

            var statisticalDays = new List<StatisticalDay>();

            var roomStatsByDay = await bookingRoomQuery
                .GroupBy(b => b.CreatedAt.Date)
                .Select(g => new StatisticalDay
                {
                    Day = g.Key,
                    TotalBooking = g.Count(),
                    TotalBookingSuccess = g.Count(x => x.Status == 5),
                    TotalBookingPending = g.Count(x => x.Status == 0 || x.Status == 1),
                    TotalBookingCancel = g.Count(x => x.Status == 2 || x.Status == 3 || x.Status == 4)
                })
                .ToListAsync();

            statisticalDays.AddRange(roomStatsByDay);


            var paymentStatsByDay = await paymentQuery
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new StatisticalDay
                {
                    Day = g.Key,
                    PaymentValue = g.Sum(p => p.Type ? p.Amount : 0),
                    RefundValue = g.Sum(p => p.Type ? 0 : p.Amount)
                })
                .ToListAsync();

            foreach (var paymentStat in paymentStatsByDay)
            {
                var existingDay = statisticalDays.FirstOrDefault(s => s.Day == paymentStat.Day);
                if (existingDay != null)
                {
                    existingDay.PaymentValue += paymentStat.PaymentValue;
                    existingDay.RefundValue += paymentStat.RefundValue;
                }
                else
                {
                    statisticalDays.Add(paymentStat);
                }
            }

            return new StatisticalResponse
            {
                TotalBooking = TotalBooking,
                TotalBookingSuccess = TotalBookingSuccess,
                TotalBookingCancel = TotalBookingCancel,
                TotalBookingPending = TotalBookingPending,
                TotalPaymentAccessValue = TotalPaymentAccessValue,
                TotalPaymentRefundValue = TotalPaymentRefundValue,
                StatisticalDay = statisticalDays.OrderBy(s => s.Day)
            };
        }

        public async Task<StatisticalResponse> TourStatiscal(StatisticalRequest request)
        {
            var bookingTourQuery = _dbContext.BookingTour
                    .Where(b => b.CreatedAt.Date >= request.Start.Date && b.CreatedAt.Date <= request.End.Date && b.TourSchedule.Tour.UserId == request.UserId)
                    .AsQueryable();

            var paymentQuery = _dbContext.Payment
                    .Where(p => p.CreatedAt.Date >= request.Start.Date && p.CreatedAt.Date <= request.End.Date && p.BookingTour.TourSchedule.Tour.UserId == request.UserId)
                    .AsQueryable();

            if (request.TourId.HasValue)
            {
                bookingTourQuery = bookingTourQuery.Where(b => b.TourSchedule.TourId == request.TourId);
                paymentQuery = paymentQuery.Where(p => p.BookingTour.TourSchedule.TourId == request.TourId);
            }

            var TotalBooking = await bookingTourQuery.CountAsync();
            var TotalBookingSuccess = await bookingTourQuery.CountAsync(x => x.Status == 5);
            var TotalBookingPending = await bookingTourQuery.CountAsync(x => x.Status == 0 || x.Status == 1);
            var TotalBookingCancel = TotalBooking - TotalBookingSuccess - TotalBookingPending;
            var TotalPaymentAccessValue = await paymentQuery.SumAsync(p => p.Type ? p.Amount : 0);
            var TotalPaymentRefundValue = await paymentQuery.SumAsync(p => p.Type ? 0 : p.Amount);

            var statisticalDays = new List<StatisticalDay>();

            var roomStatsByDay = await bookingTourQuery
                .GroupBy(b => b.CreatedAt.Date)
                .Select(g => new StatisticalDay
                {
                    Day = g.Key,
                    TotalBooking = g.Count(),
                    TotalBookingSuccess = g.Count(x => x.Status == 5),
                    TotalBookingPending = g.Count(x => x.Status == 0 || x.Status == 1),
                    TotalBookingCancel = g.Count(x => x.Status == 2 || x.Status == 3 || x.Status == 4)
                })
                .ToListAsync();

            statisticalDays.AddRange(roomStatsByDay);


            var paymentStatsByDay = await paymentQuery
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new StatisticalDay
                {
                    Day = g.Key,
                    PaymentValue = g.Sum(p => p.Type ? p.Amount : 0),
                    RefundValue = g.Sum(p => p.Type ? 0 : p.Amount)
                })
                .ToListAsync();

            foreach (var paymentStat in paymentStatsByDay)
            {
                var existingDay = statisticalDays.FirstOrDefault(s => s.Day == paymentStat.Day);
                if (existingDay != null)
                {
                    existingDay.PaymentValue += paymentStat.PaymentValue;
                    existingDay.RefundValue += paymentStat.RefundValue;
                }
                else
                {
                    statisticalDays.Add(paymentStat);
                }
            }

            return new StatisticalResponse
            {
                TotalBooking = TotalBooking,
                TotalBookingSuccess = TotalBookingSuccess,
                TotalBookingCancel = TotalBookingCancel,
                TotalBookingPending = TotalBookingPending,
                TotalPaymentAccessValue = TotalPaymentAccessValue,
                TotalPaymentRefundValue = TotalPaymentRefundValue,
                StatisticalDay = statisticalDays.OrderBy(s => s.Day)
            };
        }
    }
}
