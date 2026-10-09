using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class SectionService
    {
        private readonly ContextDb _context;

        public SectionService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAll()
        {
            var sections = await _context.Sections
                .Include(x => x.Trainer)
                .Select(x => new
                {
                    x.id_section,
                    x.name,
                    x.trainer_id,
                    trainer_name = x.Trainer != null ? x.Trainer.full_name : "Тренер не назначен",
                    x.max_participants,
                    x.schedule_time,
                    participants_count = _context.SectionEnrollments.Count(e => e.id_section == x.id_section)
                })
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = sections
            });
        }

        public async Task<ActionResult> Create(CreateSectionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.name))
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Введите название секции"
                });
            }

            if (request.trainer_id != null)
            {
                var trainer = await _context.Users
                    .FirstOrDefaultAsync(x => x.id_user == request.trainer_id && x.id_role == 2);

                if (trainer == null)
                {
                    return new JsonResult(new
                    {
                        status = false,
                        message = "Тренер не найден"
                    });
                }
            }

            if (request.max_participants != null && request.max_participants <= 0)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Лимит участников должен быть больше 0"
                });
            }

            var section = new SportSection()
            {
                name = request.name,
                trainer_id = request.trainer_id,
                max_participants = request.max_participants,
                schedule_time = request.schedule_time
            };

            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Секция успешно создана",
                section
            });
        }

        public async Task<ActionResult> Enroll(EnrollSectionRequest request)
        {
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

            if (user.id_role != 3)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Записываться в секции может только клиент"
                });
            }

            var section = await _context.Sections
                .FirstOrDefaultAsync(x => x.id_section == request.id_section);

            if (section == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Секция не найдена"
                });
            }

            var existingEnrollment = await _context.SectionEnrollments
                .FirstOrDefaultAsync(x =>
                    x.id_user == request.id_user &&
                    x.id_section == request.id_section);

            if (existingEnrollment != null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Пользователь уже записан в эту секцию"
                });
            }

            if (section.max_participants != null)
            {
                var countParticipants = await _context.SectionEnrollments
                    .CountAsync(x => x.id_section == request.id_section);

                if (countParticipants >= section.max_participants)
                {
                    return new JsonResult(new
                    {
                        status = false,
                        message = "В секции нет свободных мест"
                    });
                }
            }

            var enrollment = new SectionEnrollment()
            {
                id_user = request.id_user,
                id_section = request.id_section
            };

            await _context.SectionEnrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Пользователь успешно записан в секцию",
                enrollment
            });
        }

        public async Task<ActionResult> GetEnrollments(int id_section)
        {
            var enrollments = await _context.SectionEnrollments
                .Include(x => x.User)
                .Include(x => x.SportSection)
                .Where(x => x.id_section == id_section)
                .Select(x => new
                {
                    x.id_enrollment,
                    x.id_user,
                    user_name = x.User != null ? x.User.full_name : "",
                    x.id_section,
                    section_name = x.SportSection != null ? x.SportSection.name : ""
                })
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = enrollments
            });
        }

        public async Task<ActionResult> MarkAttendance(MarkAttendanceRequest request)
        {
            var enrollment = await _context.SectionEnrollments
                .Include(x => x.User)
                .Include(x => x.SportSection)
                .FirstOrDefaultAsync(x => x.id_enrollment == request.id_enrollment);

            if (enrollment == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Запись в секцию не найдена"
                });
            }

            var date = request.attendance_date.Date;

            var attendance = await _context.SectionAttendances
                .FirstOrDefaultAsync(x =>
                    x.id_enrollment == request.id_enrollment &&
                    x.attendance_date == date);

            if (attendance == null)
            {
                attendance = new SectionAttendance()
                {
                    id_enrollment = request.id_enrollment,
                    attendance_date = date,
                    is_present = request.is_present,
                    comment = request.comment
                };

                await _context.SectionAttendances.AddAsync(attendance);
            }
            else
            {
                attendance.is_present = request.is_present;
                attendance.comment = request.comment;
            }

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Посещаемость сохранена",
                attendance
            });
        }

        public async Task<ActionResult> GetAttendance(int id_section)
        {
            var attendance = await _context.SectionAttendances
                .Include(x => x.SectionEnrollment)
                .ThenInclude(x => x!.User)
                .Include(x => x.SectionEnrollment)
                .ThenInclude(x => x!.SportSection)
                .Where(x => x.SectionEnrollment!.id_section == id_section)
                .Select(x => new
                {
                    x.id_attendance,
                    x.id_enrollment,
                    user_name = x.SectionEnrollment!.User != null ? x.SectionEnrollment.User.full_name : "",
                    section_name = x.SectionEnrollment.SportSection != null ? x.SectionEnrollment.SportSection.name : "",
                    x.attendance_date,
                    x.is_present,
                    x.comment
                })
                .OrderByDescending(x => x.attendance_date)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = attendance
            });
        }

        public async Task<ActionResult> DeleteEnrollment(int id_enrollment)
        {
            var enrollment = await _context.SectionEnrollments
                .FirstOrDefaultAsync(x => x.id_enrollment == id_enrollment);

            if (enrollment == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Запись в секцию не найдена"
                });
            }

            _context.SectionEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Клиент удалён из секции"
            });
        }
    }
}