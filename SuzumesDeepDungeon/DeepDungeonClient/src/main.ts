import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { environment } from './environments/environment';
import { enableProdMode } from '@angular/core';


console.log('Environment:', environment);

if (environment.production) {
  enableProdMode();
  console.log('Production mode enabled');
}

bootstrapApplication(App, appConfig)
  .catch((err) => console.error(err));
