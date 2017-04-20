﻿using System;
using System.Windows.Input;
using System.Windows.Threading;
using Autofac.Extras.NLog;
using CQRS.Bus.Query;
using PictOgr.Core.Domain;
using PictOgr.GUI.SplashScreen.Commands;
using PictOgr.Infrastructure.Queries;

namespace PictOgr.GUI.SplashScreen.ViewModels
{
	public class SplashScreenViewModel : BaseViewModel
	{
		private readonly ExitApplicationCommand exitApplicationCommand;
		private readonly StartApplicationCommand startApplicationCommand;
		private DispatcherTimer startTimer;

		public ApplicationInformation ApplicationInformation { get; private set; }
		public short CurrentAutoRunTime { get; set; }
		public int TargetAutoRunTime { get; set; }

		public ICommand ExitApplicationCommand => exitApplicationCommand;
		public ICommand StartApplicationCommand => startApplicationCommand;

		public SplashScreenViewModel(
			ExitApplicationCommand exitApplicationCommand,
			StartApplicationCommand startApplicationCommand,
			IQueryBus queryBus, ILogger logger) : base(queryBus, logger)
		{
			this.exitApplicationCommand = exitApplicationCommand;
			this.startApplicationCommand = startApplicationCommand;

			Initialize();
		}

		private void Initialize()
		{
			ApplicationInformation =
				QueryBus.Process<GetApplicationInformation, ApplicationInformation>(new GetApplicationInformation());

			Logger.Info($"Start applicaiton. Version: {ApplicationInformation.Version}.");
			Logger.Info($"Show view: {GetType().Name}.");

			StartRunTimer();
		}

		private void StartRunTimer()
		{
			TargetAutoRunTime = 5;
			CurrentAutoRunTime = 0;
			startTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
			startTimer.Tick += delegate
			{
				CurrentAutoRunTime++;

				CheckTimerCountIsDone();

				OnPropertyChanged(nameof(CurrentAutoRunTime));
			};
			startTimer.Start();
		}

		private void CheckTimerCountIsDone()
		{
			if (CurrentAutoRunTime < TargetAutoRunTime)
				return;

			startTimer.Stop();

			StartApplicationCommand.Execute(this);
		}
	}
}