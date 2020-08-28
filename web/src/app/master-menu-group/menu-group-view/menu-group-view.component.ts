import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MenuGroupService } from '../../_service/menu-group.service';
import { MenuGroupView } from '../../_model/menugroup';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-menu-group-view',
  templateUrl: './menu-group-view.component.html',
  styleUrls: ['./menu-group-view.component.scss']
})
export class MenuGroupViewComponent implements OnInit {

  constructor(
    private _activateRoute: ActivatedRoute,
    private _formBuilder: FormBuilder,
    private _menuGroupSvc: MenuGroupService,
    
 
  ) { }

  public model: MenuGroupView = new MenuGroupView();
  public code : string = undefined;
   
  async ngOnInit() {
   

    this.code = this._activateRoute.snapshot.params.menuFunctionGroupId;
    //console.log(this.code);
    
     if (this.code != undefined) {
       this.model = await this._menuGroupSvc.getInfo(this.code);
     }

    
  }

  close() {
    window.history.back();
  }

}
