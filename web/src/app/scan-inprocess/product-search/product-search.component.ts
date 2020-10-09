import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthenticationService } from '../../_service/authentication.service';
// import { MessageService } from '../../_service/message.service';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';
import { DatePipe } from '@angular/common';
import { ProductSearchView } from '../../_model/job-inprocess';
import { ActivatedRoute } from '@angular/router';
// import { data } from 'jquery';

@Component({
  selector: 'app-product-search',
  templateUrl: './product-search.component.html',
  styleUrls: ['./product-search.component.scss']
})
export class ProductSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductSearchView,
    private _fb: FormBuilder,
    // private _msg: MessageService,
    // private _authSvc: AuthenticationService,
    // private _actRoute: ActivatedRoute,
    private _jobInprocessSvc: ScanInprocessService,
    private cdr: ChangeDetectorRef
  ) { }

  
  public validationForm: FormGroup;
  user:any = {};
  public model : ProductSearchView  = new ProductSearchView();
  //datas: any;
  datas: any = {};
  
  // ngAfterViewInit() {
   
  //   this.cdr.detectChanges();
  // }
 


  async ngOnInit() {
    //this.buildForm();
    var datePipe = new DatePipe("en-US");
    this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.model.pdjit_grp = this.data.pdjit_grp;
    this.model.wc_code = this.data.wc_code;
    
    this.datas = await this._jobInprocessSvc.getproduct(this.model);
    //console.log(this.datas);
    //this.search();
        
  }

  // ngAfterViewInit() {
  //   //this.datas = {};
  // }

  // ngAfterContentChecked() {
  //   this.cdr.detectChanges();
  // }

//   ngAfterContentChecked(): void {
//     this.cdr.detectChanges();
// }

  // async search() {
  //   var datePipe = new DatePipe("en-US");
  //   this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
  //   this.model.pdjit_grp = this.data.pdjit_grp;
  //   this.model.wc_code = this.data.wc_code;
    
  //   //console.log(this.model);this.cdr.detectChanges();
  //   this.datas = await this._jobInprocessSvc.getproduct(this.model);

  //   console.log(this.datas);
  // }

  // private buildForm() {
  //   this.validationForm = this._fb.group({
  //   //doc_date: [null, []]
  //   });
  // }

  // ngAfterContentChecked() {
  //   this.cdr.detectChanges();
  // }

  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);

    
    console.log(addList);

    this.dialogRef.close(addList);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}