import { ProductionRecListDetailComponent } from './../production-rec-list-detail/production-rec-list-detail.component';
import { Component, OnInit, ViewChild, ElementRef, Inject } from '@angular/core';
import { PageEvent, MatDialogRef, MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../../_service/authentication.service';
import { DatePipe } from '@angular/common'
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as moment from 'moment';
import { productionRecListDetailView, productionRecListSearchView, productionRecListTotalView } from '../../_model/production-rec-list';
import { ProductionRecListService } from '../../_service/production-rec-list.service';
import { interval, Subscription } from 'rxjs';
import { ScanReceiveService } from '../../_service/scan-receive.service';
import { search } from 'core-js/fn/symbol';
import { async } from 'rxjs/internal/scheduler/async';
import { ScanReceiveDataView, ScanReceiveDataSearchView } from '../../_model/scan-receive';

@Component({
  selector: 'app-production-rec-list',
  templateUrl: './production-rec-list.component.html',
  styleUrls: ['./production-rec-list.component.scss']
})
export class ProductionRecListComponent implements OnInit {

  // public model: productionRecListSearchView = new productionRecListSearchView();
  public model: ScanReceiveDataView = new ScanReceiveDataView();
  public searchModel: ScanReceiveDataSearchView = new ScanReceiveDataSearchView();
  // public datas: productionRecListTotalView<productionRecListDetailView> = new productionRecListTotalView<productionRecListDetailView>();
  public datas: any = {};
  public data: any = {};
  public user: any; 
  public datePipe = new DatePipe('en-US'); 
  public validationForm: FormGroup;
  public time_delay : any;
  public docDate : any;
  private updateSubscription: Subscription;
  public chkRefresh : any;
  public data_docdate : any;

  @ViewChild('doc_date') doc_date: ElementRef;

  constructor(
    private _productionRecListSvc: ProductionRecListService,
    private _authSvc: AuthenticationService,
    private _activateRoute: ActivatedRoute,
    private _router: Router,
    private _formBuilder: FormBuilder,
    private _dialog: MatDialog,
    private _scanRecSvc: ScanReceiveService,
  ) { }

  ngOnInit() {
    //this.buildForm();
    this.user = this._authSvc.getLoginUser(); 
    
    
    this.docDate = new Date();
    this.searchModel.doc_date = this.docDate;
    this.searchModel.send_type = "WAIT";
    this.searchProductionRecList();

    
  }

  async onRefrechChanged(value)
  {
    console.log(value);
    if(value==true)
    {
      this.time_delay = await this._productionRecListSvc.getTimeDelay('H10','WHRPD');
      this.time_delay = this.time_delay * 1000;
      console.log(this.time_delay);
      this.updateSubscription = interval(this.time_delay).subscribe(
        (val) => { this.searchProductionRecList()});
    }
    else
    {
      if (this.updateSubscription) {
            this.updateSubscription.unsubscribe();
          }
    }
  }

 

  close() { 
    this._router.navigateByUrl('/app/mobile-navigator');  
  }
  
  ngOnDestroy() {
    if (this.updateSubscription) {
          this.updateSubscription.unsubscribe();
        }
  
  }

  async searchProductionRecList(event: PageEvent = null) {   

    
      if (event != null) {
        this.model.pageIndex = event.pageIndex;
        this.model.itemPerPage = event.pageSize;
      }
  
      this.data_docdate = this.searchModel.doc_date;
      
       
      var datePipe = new DatePipe("en-US");
      this.searchModel.doc_date = datePipe.transform(this.searchModel.doc_date, 'dd/MM/yyyy').toString();
      this.searchModel.user_id = this.user.username;
      this.searchModel.build_type = this.user.branch.entity_code;
  
      console.log(this.searchModel);
      this.data =  await this._scanRecSvc.searchDataScanReceive(this.searchModel);
      console.log(this.data);
     
      // this.fin_date.nativeElement.value = this.data_findate;
      this.searchModel.doc_date = this.data_docdate;

      
      // if (event != null) {
      //   this.model.pageIndex = event.pageIndex;
      //   this.model.itemPerPage = event.pageSize;
      // }
      // var datePipe = new DatePipe("en-US");
      // this.datas.recDetails  = [];
      // this.model.build_type   = this.user.branch.entity_code;
      
      // this.model.doc_date = datePipe.transform(this.model.doc_date, 'dd/MM/yyyy').toString();
      // this.model.doc_no       = "";
      // this.model.doc_status   = "";
      // console.log(this.model.doc_date);
      
      
      // this.datas =  await this._productionRecListSvc.searchProductionRecList(this.model);
      // console.log(this.datas);
      // this.doc_date.nativeElement.value = this.model.doc_date;
   
  } 


//   async searchProductionRecRefreshList(event: PageEvent = null) {   

//     if (event != null) {
//       this.model.pageIndex = event.pageIndex;
//       this.model.itemPerPage = event.pageSize;
//     }
//     var datePipe = new DatePipe("en-US");
//     this.datas.recDetails  = [];
//     this.model.build_type   = this.user.branch.entity_code;
//     this.model.doc_date     = this.doc_date.nativeElement.value; // 16/10/2020
//     // this.model.doc_date = datePipe.transform(this.model.doc_date, 'dd/MM/yyyy').toString();
//     this.model.doc_no       = "";
//     this.model.doc_status   = "";
//     console.log(this.model.doc_date);
//     //this.model.doc_date     = "";

//     this.datas =  await this._productionRecListSvc.searchProductionRecList(this.model);
//     console.log(this.datas);
//     this.doc_date.nativeElement.value = this.model.doc_date;
  
   
// } 

openProductDetail(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ProductionRecListDetailComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        doc_no: p_doc_no,
        entity:p_entity
       
      }

    });

    // dialogRef.afterClosed().subscribe(result => {
    //   if (result.length > 0) {
    //     //this.add_prod(result);
    //   }
    // })
  }

  // openProductionRecListDetailDialog(p_docNo: string, p_docDate: string, _isEdit: boolean = false, _index: number = -1) {
  //   const dialogRef = this._dialog.open(ProductionRecListDetailComponent, {
  //     maxWidth: '100vw',
  //     maxHeight: '100vh', 
  //     height: '100%',
  //     width: '100%',
  //     data: {
  //       doc_no: p_docNo,
  //       doc_date: p_docDate,
  //       isEdit: _isEdit,
  //      // editItem: _editItem,
  //       hideSerialNo: true,
  //       isSaleBed: false
  //     }
  //   });
  
  //   dialogRef.afterClosed().subscribe(result => { 
  //     if (result) {
        
  //     }
  //   });
  // }





}


export class YourValidator {
  static dateVaidator(AC: AbstractControl) {

    if (AC && AC.value && !moment(AC.value, 'YYYY-MM-DD',true).isValid()) {
      return {'dateVaidator': true};
    }
    return null;  
  }
}
