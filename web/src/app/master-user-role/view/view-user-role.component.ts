import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../_common/base.component';
import { MessageService } from '../../_service/message.service';
import { ActivatedRoute } from '@angular/router';
import { UserRoleService } from '../../_service/user-role.service';

@Component({
  selector: 'app-view-user-role',
  templateUrl: './view-user-role.component.html',
  styleUrls: ['./view-user-role.component.scss']
})
export class ViewUserRoleComponent extends BaseComponent implements OnInit {

  public functionGroup: any = [];

  public data: any = {};

  constructor(
    public messageService: MessageService,
    private route: ActivatedRoute,
    private userRoleService: UserRoleService
  ) {
    super();
  }

  async ngOnInit() {

    this.data = await this.userRoleService.get(this.route.snapshot.params.id);

    this.functionGroup = await this.userRoleService.getFunctionGroup(this.data.isPC);
    console.log(this.data);
    console.log(this.functionGroup);

    this.data.userRoleFunctionAuthorizationList.forEach(userRoleFunctionAuthorization => {
      userRoleFunctionAuthorization.userRoleFunctionAccessList.forEach(userRoleFunctionAccess => {


        this.functionGroup.forEach(group => {
          group.menuFunctionList.forEach(func => {
            func.menuFunctionActionList.forEach(action => {
              if( action.menuFunctionActionId == userRoleFunctionAccess.menuFunctionActionId ){
                action.isSelected = true;
              }
            });
            
          });
        });


      });
      
    });

    this.functionGroup.forEach(group => {

      let checkAll = true;

      group.menuFunctionList.forEach(func => {
        func.menuFunctionActionList.forEach(action => {
          if( !action.isSelected ){
                  checkAll = false;
                }
        });
        
      });
      group.isAll = checkAll;
    });

  }

}
