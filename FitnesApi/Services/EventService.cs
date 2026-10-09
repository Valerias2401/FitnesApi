using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class EventService
    {
        private readonly ContextDb _context;

        public EventService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAll()
        {
            var events = await _context.SportEvents
                .Include(x => x.Court)
                .Select(x => new
                {
                    x.id_event,
                    x.name,
                    x.description,
                    x.event_date,
                    x.id_court,
                    court_name = x.Court != null ? x.Court.name : "",
                    x.max_participants,
                    x.bracket_info,
                    x.game_schedule,
                    x.photo_gallery,
                    x.status,
                    participants_count = _context.EventParticipants.Count(p => p.id_event == x.id_event)
                })
                .OrderByDescending(x => x.event_date)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = events
            });
        }

        public async Task<ActionResult> Create(CreateEventRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.name))
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Введите название мероприятия"
                });
            }

            var court = await _context.Courts
                .FirstOrDefaultAsync(x => x.id_court == request.id_court);

            if (court == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Площадка не найдена"
                });
            }

            if (request.max_participants <= 0)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Количество участников должно быть больше 0"
                });
            }

            var sportEvent = new SportEvent()
            {
                name = request.name,
                description = request.description,
                event_date = request.event_date,
                id_court = request.id_court,
                max_participants = request.max_participants,
                bracket_info = string.Empty,
                game_schedule = string.Empty,
                photo_gallery = string.Empty,
                status = "planned"
            };

            await _context.SportEvents.AddAsync(sportEvent);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Мероприятие успешно создано",
                sportEvent
            });
        }

        public async Task<ActionResult> UpdateOrganization(UpdateEventOrganizationRequest request)
        {
            var sportEvent = await _context.SportEvents
                .FirstOrDefaultAsync(x => x.id_event == request.id_event);

            if (sportEvent == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Мероприятие не найдено"
                });
            }

            sportEvent.bracket_info = request.bracket_info;
            sportEvent.game_schedule = request.game_schedule;
            sportEvent.photo_gallery = request.photo_gallery;

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Данные соревнования сохранены",
                sportEvent
            });
        }

        public async Task<ActionResult> AddParticipant(EventParticipantRequest request)
        {
            var sportEvent = await _context.SportEvents
                .FirstOrDefaultAsync(x => x.id_event == request.id_event);

            if (sportEvent == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Мероприятие не найдено"
                });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.id_user == request.id_user);

            if (user == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Пользователь не найден"
                });
            }

            var existingParticipant = await _context.EventParticipants
                .FirstOrDefaultAsync(x =>
                    x.id_event == request.id_event &&
                    x.id_user == request.id_user);

            if (existingParticipant != null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Пользователь уже записан на мероприятие"
                });
            }

            var countParticipants = await _context.EventParticipants
                .CountAsync(x => x.id_event == request.id_event);

            if (countParticipants >= sportEvent.max_participants)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "На мероприятии нет свободных мест"
                });
            }

            var participant = new EventParticipant()
            {
                id_event = request.id_event,
                id_user = request.id_user
            };

            await _context.EventParticipants.AddAsync(participant);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Участник успешно добавлен",
                participant
            });
        }

        public async Task<ActionResult> DeleteParticipant(int id_participant)
        {
            var participant = await _context.EventParticipants
                .FirstOrDefaultAsync(x => x.id_participant == id_participant);

            if (participant == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Участник не найден"
                });
            }

            _context.EventParticipants.Remove(participant);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Участник удалён из мероприятия"
            });
        }

        public async Task<ActionResult> GetParticipants(int id_event)
        {
            var participants = await _context.EventParticipants
                .Include(x => x.User)
                .Where(x => x.id_event == id_event)
                .Select(x => new
                {
                    x.id_participant,
                    x.id_event,
                    x.id_user,
                    user_name = x.User != null ? x.User.full_name : "",
                    user_email = x.User != null ? x.User.email : "",
                    user_phone = x.User != null ? x.User.phone : ""
                })
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = participants
            });
        }

        public async Task<ActionResult> CreateGame(CreateEventGameRequest request)
        {
            var sportEvent = await _context.SportEvents
                .FirstOrDefaultAsync(x => x.id_event == request.id_event);

            if (sportEvent == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Мероприятие не найдено"
                });
            }

            if (string.IsNullOrWhiteSpace(request.first_participant) ||
                string.IsNullOrWhiteSpace(request.second_participant))
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Введите участников игры"
                });
            }

            var game = new EventGame()
            {
                id_event = request.id_event,
                first_participant = request.first_participant,
                second_participant = request.second_participant,
                game_time = request.game_time,
                result = "Не сыграно"
            };

            await _context.EventGames.AddAsync(game);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Игра добавлена в расписание",
                game
            });
        }

        public async Task<ActionResult> GetGames(int id_event)
        {
            var games = await _context.EventGames
                .Where(x => x.id_event == id_event)
                .OrderBy(x => x.game_time)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = games
            });
        }

        public async Task<ActionResult> UpdateGameResult(UpdateEventGameResultRequest request)
        {
            var game = await _context.EventGames
                .FirstOrDefaultAsync(x => x.id_game == request.id_game);

            if (game == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Игра не найдена"
                });
            }

            game.result = request.result;

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Результат игры сохранён",
                game
            });
        }

        public async Task<ActionResult> DeleteGame(int id_game)
        {
            var game = await _context.EventGames
                .FirstOrDefaultAsync(x => x.id_game == id_game);

            if (game == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Игра не найдена"
                });
            }

            _context.EventGames.Remove(game);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Игра удалена"
            });
        }

        public async Task<ActionResult> AddResult(CreateEventResultRequest request)
        {
            var sportEvent = await _context.SportEvents
                .FirstOrDefaultAsync(x => x.id_event == request.id_event);

            if (sportEvent == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Мероприятие не найдено"
                });
            }

            var result = new EventResult()
            {
                id_event = request.id_event,
                result_text = request.result_text
            };

            sportEvent.status = "completed";

            await _context.EventResults.AddAsync(result);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Результат мероприятия опубликован",
                result
            });
        }

        public async Task<ActionResult> GetResults()
        {
            var results = await _context.EventResults
                .Include(x => x.SportEvent)
                .Select(x => new
                {
                    x.id_result,
                    x.id_event,
                    event_name = x.SportEvent != null ? x.SportEvent.name : "",
                    x.result_text
                })
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = results
            });
        }
    }
}