﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelsController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var hotels = await _hotelService.GetAll();
                return StatusCode(200, hotels);
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var hotel = await _hotelService.GetById(id);
                return StatusCode(200, hotel);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("destination/{id}")]
        public async Task<IActionResult> GetByDestination(int id)
        {
            try
            {
                var hotels = await _hotelService.GetByDestination(id);
                return StatusCode(200, hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpGet("partner/{id}")]
        public async Task<IActionResult> GetByPartner(Guid id)
        {
            try
            {
                var hotels = await _hotelService.GetByPartner(id);
                return StatusCode(200, hotels);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
        {
            try
            {
                await _hotelService.CreateHotel(hotel);
                return StatusCode(201, "create hotel successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPut("edit")]
        public async Task<IActionResult> UpdateHotel([FromQuery] Guid hotelId, [FromBody] Hotel hotel)
        {
            try
            {
                var result = await _hotelService.UpdateHotel(hotelId, hotel);
                if (!result)
                {
                    return StatusCode(400, "update failed, no field change");
                }
                return StatusCode(200, "update hotel successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPost("edit/upload-image")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files,[FromQuery] Guid hotelId)
        {
            try
            {
                var result = await _hotelService.UploadImagesAsync(files, hotelId);
                if (!result) return StatusCode(400, "Failed to upload images.");
                return StatusCode(201, "upload image successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchHotel([FromBody] SearchHotelRequest request)
        {
            try
            {
                var hotels = await _hotelService.SearchHotel(request);
                return StatusCode(200, hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
                
    }
}
