using AplikacjaWebowa.Data;
using AplikacjaWebowa.Models;
using AplikacjaWebowa.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaWebowa.Controllers
{
    [Authorize]
    public class SurveysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SurveysController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var surveys = await _context.Surveys
                .Include(s => s.Options)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(surveys);
        }

        [Authorize(Roles = "Ankieter")]
        public IActionResult Create()
        {
            return View(new CreateSurveyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Create(CreateSurveyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            var survey = new Survey
            {
                Question = model.Question.Trim(),
                CreatorId = userId,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            survey.Options.Add(new Option
            {
                Text = model.Option1.Trim()
            });

            survey.Options.Add(new Option
            {
                Text = model.Option2.Trim()
            });

            if (!string.IsNullOrWhiteSpace(model.Option3))
            {
                survey.Options.Add(new Option
                {
                    Text = model.Option3.Trim()
                });
            }

            if (!string.IsNullOrWhiteSpace(model.Option4))
            {
                survey.Options.Add(new Option
                {
                    Text = model.Option4.Trim()
                });
            }

            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Ankieta została utworzona.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            bool alreadyVoted = await _context.Votes
                .AnyAsync(v =>
                    v.RespondentId == userId &&
                    v.Option.SurveyId == id);

            var model = new VoteViewModel
            {
                SurveyId = survey.Id,
                Question = survey.Question,
                Options = survey.Options,
                AlreadyVoted = alreadyVoted,
                IsActive = survey.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vote(VoteViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            var survey = await _context.Surveys
                .Include(s => s.Options)
                .FirstOrDefaultAsync(s => s.Id == model.SurveyId);

            if (survey == null)
            {
                return NotFound();
            }

            if (!survey.IsActive)
            {
                TempData["ErrorMessage"] =
                    "Ta ankieta jest obecnie nieaktywna.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.SurveyId });
            }

            bool alreadyVoted = await _context.Votes
                .AnyAsync(v =>
                    v.RespondentId == userId &&
                    v.Option.SurveyId == model.SurveyId);

            if (alreadyVoted)
            {
                TempData["ErrorMessage"] =
                    "Oddałeś już głos w tej ankiecie.";

                return RedirectToAction(
                    nameof(Details),
                    new { id = model.SurveyId });
            }


            if (!model.SelectedOptionId.HasValue)
            {
                ModelState.AddModelError(
                    nameof(model.SelectedOptionId),
                    "Wybierz jedną odpowiedź.");
            }

            var selectedOption = survey.Options
                .FirstOrDefault(o => o.Id == model.SelectedOptionId);

            if (selectedOption == null)
            {
                ModelState.AddModelError(
                    nameof(model.SelectedOptionId),
                    "Wybrana odpowiedź jest nieprawidłowa.");
            }

            if (!ModelState.IsValid)
            {
                model.Question = survey.Question;
                model.Options = survey.Options;
                model.AlreadyVoted = false;
                model.IsActive = survey.IsActive;

                return View("Details", model);
            }

            var vote = new Vote
            {
                OptionId = selectedOption!.Id,
                RespondentId = userId,
                VotedAt = DateTime.Now
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Twój głos został zapisany.";

            return RedirectToAction(
                nameof(Details),
                new { id = model.SurveyId });
        }
        public async Task<IActionResult> Results(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            int totalVotes = survey.Options
                .Sum(o => o.Votes.Count);

            var model = new SurveyResultsViewModel
            {
                SurveyId = survey.Id,
                Question = survey.Question,
                TotalVotes = totalVotes,
                Options = survey.Options.Select(o => new OptionResultViewModel
                {
                    Text = o.Text,
                    VoteCount = o.Votes.Count,
                    Percentage = totalVotes == 0
                        ? 0
                        : Math.Round(
                            (double)o.Votes.Count / totalVotes * 100,
                            1)
                }).ToList()
            };

            return View(model);
        }
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Votes(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Edit(int id)
        {
            var survey = await _context.Surveys
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            var model = new EditSurveyViewModel
            {
                Id = survey.Id,
                Question = survey.Question,
                IsActive = survey.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Edit(EditSurveyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var survey = await _context.Surveys
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (survey == null)
            {
                return NotFound();
            }

            survey.Question = model.Question.Trim();
            survey.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ankieta została zaktualizowana.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Delete(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
            {
                return NotFound();
            }

            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Ankieta została usunięta.";

            return RedirectToAction(nameof(Index));
        }
    }
}