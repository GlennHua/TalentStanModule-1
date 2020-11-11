/* Experience section */
import React from 'react';
import Cookies from 'js-cookie';
import {Button, Form, Tab, Table} from 'semantic-ui-react';
//import DatePicker from 'react-datepicker';
import moment from 'moment';
import { ChildSingleInput } from '../Form/SingleInput.jsx';

export default class Experience extends React.Component {

    constructor(props) {

        super(props);

        this.state = {

            showAddSection : false,

            expList : [],

            newExp : {

                Id : '',
                Company : '',
                Position : '',
                Responsibilities : '',
                Start : '',
                End : ''
            },

            status : ''

        }
       


        this.renderTable = this.renderTable.bind(this)
        this.renderAddSection = this.renderAddSection.bind(this)
        this.openAddSection = this.openAddSection.bind(this)
        this.closeAddSection = this.closeAddSection.bind(this)
        this.renderBothForAdd = this.renderBothForAdd.bind(this)
        this.getExperience = this.getExperience.bind(this)
        this.addExperience = this.addExperience.bind(this)
        this.handleAddInput = this.handleAddInput.bind(this)
        this.deleteExp = this.deleteExp.bind(this)
        this.openEditSection = this.openEditSection.bind(this)
        this.CheckEditOrAdd = this.CheckEditOrAdd.bind(this)
        this.updateExp = this.updateExp.bind(this)

    };
    
    componentDidMount()
    {
        this.getExperience()
        //this.addExperience()
    }

    openAddSection()
    {
        const emptyExp = {

            Id : '',
            Company : '',
            Position : '',
            Responsibilities : '',
            Start : '',
            End : ''

        }

        this.setState({

            showAddSection : true,
            newExp : emptyExp,
            status : 'add'

        })
    }

    closeAddSection()
    {
        this.setState({
            showAddSection : false
        })
    }

    handleAddInput(event)
    {
        const data = Object.assign({}, this.state.newExp)
        
        data[event.target.name] = event.target.value

        this.setState({

            newExp : data

        })
        console.log(data)
    }

    openEditSection(ExpObj)
    {
        const emptyExp = {

                Id : '',
                Company : '',
                Position : '',
                Responsibilities : '',
                Start : '',
                End : ''
        }

        const data = Object.assign({}, ExpObj)

        emptyExp.Id = data.id
        emptyExp.Company = data.company
        emptyExp.Position = data.position
        emptyExp.Responsibilities = data.responsibilities
        emptyExp.Start = moment(data.start).format('DD/MM/YYYY')
        emptyExp.End = moment(data.end).format('DD/MM/YYYY')

        this.setState({
            newExp : emptyExp,
            showAddSection : true,
            status : 'edit'
        })

    }

    CheckEditOrAdd(exp, EditOrAdd)
    {

        EditOrAdd == 'Add' ? this.openAddSection() : this.openEditSection(exp)

    }

    getExperience()
    {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/GetExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            success: function (res) {

                //console.log(res)

                res ? console.log(res.data) : console.log('Nothing returned')

                this.setState({

                    expList : res.data

                })


            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
            }
        })
    }



    addExperience()
    {
        const data = Object.assign({}, this.state.newExp)

        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/AddExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data : JSON.stringify(data),
            dataType: "json",
            success: function (res) {

                //console.log(res)

                res ? console.log(res.data) : console.log('Nothing returned')


            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
            }
        })

    }


    deleteExp(ExpId)
    {

        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/DeleteExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data : JSON.stringify(ExpId),
            dataType: "json",
            success: function (res) {

                res ? console.log(res) : console.log('Nothing returned')

            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
            }
        })

    }

    updateExp()
    {
        const data = Object.assign({}, this.state.newExp)

        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/UpdateExperience',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data : JSON.stringify(data),
            dataType: "json",
            success: function (res) {

                res ? console.log(res) : console.log('Nothing returned')

            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
            }
        })

    }



    renderAddSection()
    {

        return(

            <div className = 'ui sixteen wide column'>

            
                <Form.Group widths = 'equal'>

                    <ChildSingleInput 
                        label = 'Company'
                        placeholder = 'Company' 
                        name = 'Company'
                        value = {this.state.newExp.Company}
                        maxLength = {50}
                        inputType = 'text'
                        errorMessage = 'Please Enter Valid Info'
                        controlFunc = {this.handleAddInput}
                        minLength = {10}
                    />

                    <ChildSingleInput 
                        label = 'Position'
                        placeholder = 'Position' 
                        name = 'Position'
                        value = {this.state.newExp.Position}
                        maxLength = {50}
                        inputType = 'text'
                        errorMessage = 'Please Enter Valid Info'
                        controlFunc = {this.handleAddInput}
                    />

                </Form.Group>

                <Form.Group widths = 'equal'>

                    <ChildSingleInput 
                        label = 'Start Date'
                        placeholder = 'DD/MM/YYYY' 
                        name = 'Start'
                        value = {this.state.newExp.Start}
                        maxLength = {30}
                        inputType = 'text'
                        errorMessage = 'Please Enter Valid Info'
                        controlFunc = {this.handleAddInput}
                    />
                   
                   <ChildSingleInput 
                        label = 'End Date' 
                        placeholder = 'DD/MM/YYYY' 
                        name = 'End'
                        value = {this.state.newExp.End}
                        maxLength = {30}
                        inputType = 'text'
                        errorMessage = 'Please Enter Valid Info'
                        controlFunc = {this.handleAddInput}
                    />


                </Form.Group>

                <Form.Group widths = 'equal'>

                    <ChildSingleInput 
                        label = 'Responsibilities' 
                        placeholder = 'Responsibilities' 
                        name = 'Responsibilities'
                        value = {this.state.newExp.Responsibilities}
                        maxLength = {150}
                        inputType = 'text'
                        errorMessage = 'Please Enter Valid Info'
                        controlFunc = {this.handleAddInput}
                    />
                    
                </Form.Group>

                {this.state.status == 'add' ? 
                
                <Button 
                    color = 'black'
                    content = 'Add'
                    onClick = {this.addExperience}
                />

                :

                <Button
                    color = 'black'
                    content = 'Save'
                    onClick = {this.updateExp}
                />
            
                }

                {/* <Button 
                    color = 'black'
                    content = 'Add'
                    onClick = {this.addExperience}
                /> */}

                <Button 
                    content = 'Cancel'
                    onClick = {this.closeAddSection}
                />

            
            </div>

        )

    }


    renderTable()
    {

        const expList = this.state.expList


        return(

            <div className = 'ui sixteen wide column'>
            <Table>
                <Table.Header>
                    <Table.Row>

                        <Table.HeaderCell>Company</Table.HeaderCell>
                        <Table.HeaderCell>Position</Table.HeaderCell>
                        <Table.HeaderCell>Responsibilities</Table.HeaderCell>
                        <Table.HeaderCell>Start</Table.HeaderCell>
                        <Table.HeaderCell>End</Table.HeaderCell>
                        <Table.HeaderCell>
                            <Button 
                                content = 'Add New'
                                icon = 'add'
                                floated = 'right'
                                color = 'black'
                                onClick = {()=>this.CheckEditOrAdd('empty', 'Add')}
                            />
                        </Table.HeaderCell>

                    </Table.Row>
                </Table.Header>

                <Table.Body>

                    {
                        expList.map(
                            (exp, key)=>{

                                return(
                                <Table.Row key = {key}>
                                    <Table.Cell>{exp.company}</Table.Cell>
                                    <Table.Cell>{exp.position}</Table.Cell>
                                    <Table.Cell>{exp.responsibilities}</Table.Cell>
                                    <Table.Cell>{moment(exp.start).format("Do MMM, YYYY")}</Table.Cell>
                                    <Table.Cell>{moment(exp.end).format("Do MMM, YYYY")}</Table.Cell>
                                    <Table.Cell>

                                        <Button 
                                            color = 'black'
                                            icon = 'cancel'
                                            floated = 'right'
                                            size = 'tiny'
                                            onClick = {()=>this.deleteExp(exp.id)}
                                        />
                                        <Button 
                                            color = 'black'
                                            icon = 'pencil'
                                            floated = 'right'
                                            size = 'tiny'
                                            onClick = {()=> this.openEditSection(exp)}
                                            // onClick = {()=> this.CheckEditOrAdd(exp, 'Edit')} 
                                        />

                                    </Table.Cell>

                            </Table.Row>
                            )   
                        }
                        )
                    }

                    


                </Table.Body>




            </Table>
            </div>

        )

    }


    renderBothForAdd()
    {
        return(
            
        <div className = 'ui sixteen wide column'>

            {this.renderAddSection()}
            {this.renderTable()}

        </div>
        )
        
    }

 

    render() 
    {


        return(

            this.state.showAddSection ? this.renderBothForAdd() : this.renderTable()

        )
        
    }
}
