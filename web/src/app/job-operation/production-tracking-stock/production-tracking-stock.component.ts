import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ProductGroupSearchView, ProductionTrackingStockSearchView } from '../../_model/job-operation-stock';
import { AuthenticationService } from '../../_service/authentication.service';
import { JobOperationStockService } from '../../_service/job-operation-stock.service';
import { MessageService } from '../../_service/message.service';
import { DatePipe } from '@angular/common';
import { ProductionTrackingGroupComponent } from '../production-tracking-group/production-tracking-group.component';

@Component({
  selector: 'app-production-tracking-stock',
  templateUrl: './production-tracking-stock.component.html',
  styleUrls: ['./production-tracking-stock.component.scss']
})
export class ProductionTrackingStockComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductionTrackingStockSearchView,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _jobSvc: JobOperationStockService,
  ) { }

  public user: any;
  public searchModel: ProductionTrackingStockSearchView = new ProductionTrackingStockSearchView();
  public datas: any = {};
  public datas_det: any = {};
  // public datePipe = new DatePipe('en-US');

  ngOnInit() {
    this.user = this._authSvc.getLoginUser();
    this.searchProductionTracking();
  }
  async searchProductionTracking()
  {
    var datePipe = new DatePipe("en-US");

    console.log(this.data);
    this.searchModel.entity_code = this.data.entity_code;
    this.searchModel.req_date  =  datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString(); 
    this.searchModel.wc_code = this.data.wc_code;
    this.searchModel.por_no = this.data.por_no;
    this.searchModel.ref_no = this.data.ref_no;
    this.searchModel.build_type = this.user.branch.entity_code;
    this.searchModel.user_id = this.data.user_id;

    console.log(this.searchModel);
    this.datas =  await this._jobSvc.searchProductionTrackingStock(this.searchModel);
    this.datas_det =  await this._jobSvc.searchProductionTrackingDetailStock(this.searchModel);
    //console.log(this.datas);
  }

  openProductionGroupTracking(p_entity : string ,p_por_no: string,p_ref_no: string , p_req_date : string  , p_group : string, _index: number = -1)
  {
    console.log(this.searchModel.entity_code);
    console.log(this.searchModel.por_no);
    console.log(this.searchModel.ref_no);
    console.log(this.searchModel.req_date);
    console.log(p_group);
    console.log(this.user.username);
    
    const dialogRef = this._dialog.open(ProductionTrackingGroupComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        por_no: this.searchModel.por_no,
        entity_code:this.searchModel.entity_code,
        ref_no :this.searchModel.ref_no,
        req_date : p_req_date,
        user_id : this.user.username,
        group : p_group,
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     //this.add_prod(result);
    //   }
    // })
  }
  
  close() { 
    this.dialogRef.close();
  }

}
