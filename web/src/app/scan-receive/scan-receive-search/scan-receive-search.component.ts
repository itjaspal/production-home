import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, PageEvent } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanApproveSendCreateComponent } from '../../scan-approve-send/scan-approve-send-create/scan-approve-send-create.component';
import { ScanReceiveDataView, ScanReceiveDataSearchView, ConfirmStockDataView } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';
import { ScanCheckQrComponent } from '../scan-check-qr/scan-check-qr.component';
import { ScanReceiveCreateComponent } from '../scan-receive-create/scan-receive-create.component';
import { ScanReceiveProductViewComponent } from '../scan-receive-product-view/scan-receive-product-view.component';

@Component({
  selector: 'app-scan-receive-search',
  templateUrl: './scan-receive-search.component.html',
  styleUrls: ['./scan-receive-search.component.scss']
})
export class ScanReceiveSearchComponent implements OnInit {

  @ViewChild('doc_date') doc_date: ElementRef;

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanReceiveDataView = new ScanReceiveDataView();
  public searchModel: ScanReceiveDataSearchView = new ScanReceiveDataSearchView();
  public confirmModel : ConfirmStockDataView = new ConfirmStockDataView();

  public data: any = {};
  // public model_scan: ScanSendFinView = new ScanSendFinView();
  // public model_setno : PrintSetNoView = new PrintSetNoView();
  public datas: any = {};
  public datas_print: any = {};
  public data_qty : any;
  public data_docdate : any;
  public docDate : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    this.docDate = new Date();
    this.searchModel.doc_date = this.docDate;
    this.searchModel.send_type = "WAIT";
  
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      doc_no: [null, []],
      doc_date: [null, [Validators.required]],
      //send_type:[null, [Validators.required]]
    });
  }
  
  async search(event: PageEvent = null) {   
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
   
}


  openScanReceiveNew(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    console.log(p_entity);
    const dialogRef = this._dialog.open(ScanReceiveCreateComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        doc_no: p_doc_no,
        entity:p_entity
       
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      this.search();
      // if (result.length > 0) {
      //   //this.add_prod(result);
      // }
    })
  }

  openProductDetail(p_entity : string ,p_doc_no: string  , _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanReceiveProductViewComponent, {
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


  openScanCheckQr()
  {
    
    const dialogRef = this._dialog.open(ScanCheckQrComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
              
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      this.search();
      // if (result.length > 0) {
      //   //this.add_prod(result);
      // }
    })
  }

  radioTypeChange(value) {
    this.searchModel.send_type = value;
    
    console.log(this.searchModel.send_type);
  }


  async ConfirmStock(entity,doc_no)
  {
    console.log(entity);
    this.confirmModel.entity = entity;
    this.confirmModel.doc_no = doc_no;
    this.confirmModel.user_id = this.user.username;

    console.log(this.confirmModel);

    this._msgSvc.confirmPopup("ยืนยัน Update Stock", async result => {
      if (result) {
        let res: any = await this._scanRecSvc.ConfirmStock(this.confirmModel);

        this._msgSvc.successPopup(res.message);
        this.search();
      }
      
    })

    this.search();

 
  }

  close() {
    this._router.navigateByUrl('/app/home');
  }


}
