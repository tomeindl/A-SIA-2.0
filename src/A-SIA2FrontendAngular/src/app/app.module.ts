import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DataPanelModule } from './modules/data-panel/data-panel.module';
import { GraphEditorModule } from './modules/graph-editor/graph-editor.module';
import { MenubarModule } from './modules/menubar/menubar.module';
import { SimulationDiagramModule } from './modules/simulation-diagram/simulation-diagram.module';
import { SharedModule } from './shared/shared.module';
import { LoginComponent } from './modules/login/login.component';

@NgModule({
  declarations: [AppComponent, LoginComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DataPanelModule,
    MenubarModule,
    GraphEditorModule,
    SimulationDiagramModule,
    SharedModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
