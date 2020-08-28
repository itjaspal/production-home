import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { MenuService } from '../../_service/menu.service';
import { MenuView } from '../../_model/menu';

@Component({
  selector: 'app-menu-view',
  templateUrl: './menu-view.component.html',
  styleUrls: ['./menu-view.component.scss']
})
export class MenuViewComponent implements OnInit {

  constructor(
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _menuSvc: MenuService
    
 
  ) { }

  public model: MenuView = new MenuView();
  public code : string = undefined;
   
  async ngOnInit() {
   

    this.code = this._activateRoute.snapshot.params.menuFunctionId;
    //console.log(this.code);
    
     if (this.code != undefined) {
       this.model = await this._menuSvc.getInfo(this.code);
     }

    
  }

  close() {
    window.history.back();
  }

}
