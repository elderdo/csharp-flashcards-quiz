using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizletApp.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(args);
builder.Services.AddSingleton<IUserInteraction, ConsoleUserInteraction>();
builder.Services.AddSingleton<IQuizSettingsProvider, QuizSettingsProvider>();
builder.Services.AddSingleton<ISeparatorProvider, SeparatorProvider>();
builder.Services.AddSingleton<IFlashcardParser, FlashcardParser>();
builder.Services.AddSingleton<IFlashcardLoader, FlashcardLoader>();
builder.Services.AddSingleton<IQuizCatalogService, QuizCatalogService>();
builder.Services.AddSingleton<QuizApplication>();

using var host = builder.Build();
await host.Services.GetRequiredService<QuizApplication>().RunAsync();
