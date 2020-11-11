/* Language section */
import React, {Component} from 'react';
import Cookies from 'js-cookie';
import { Button, Tab, Table, Input, Form, Dropdown } from 'semantic-ui-react';

export default class Language extends Component {
   
   
    constructor(props) {
       
        super(props);

        this.state = {

            showAddSection : false,

            showEditSection : false,

            Languages: [],

            newLanguage : {

                Name : '',
                Level : ''

            },

            Name : '',
            Level : '',
            targetId : '',


            LanguageForEdit : {

                Name: '', 
                Level: '', 
                Id: '', 
                CurrentUserId: null

            },

            nameForEdit : '',
            levelForEdit : ''

            

        }
       
        this.renderAdd = this.renderAdd.bind(this)
        this.openAdd = this.openAdd.bind(this)
        this.closeAdd = this.closeAdd.bind(this)

        this.renderTable = this.renderTable.bind(this)
        this.renderBoth = this.renderBoth.bind(this)
        this.getLanguages = this.getLanguages.bind(this)
        this.updateLanguage = this.updateLanguage.bind(this)
        this.handleInput = this.handleInput.bind(this)
        this.handleDropDownChange = this.handleDropDownChange.bind(this)
        this.DeleteLanguage = this.DeleteLanguage.bind(this)
        this.OpenEdit = this.OpenEdit.bind(this)
        this.CloseEdit = this.CloseEdit.bind(this)
        this.EditLanguage = this.EditLanguage.bind(this)
        this.handleInputEdit = this.handleInputEdit.bind(this)
        this.handleDropDownEdit = this.handleDropDownEdit.bind(this)

    }

    componentDidMount()
    {
        this.getLanguages();
        // this.EditLanguage();
        // console.log('Testing')
    }

    getLanguages()
    {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/GetLanguages',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            success: function (res) {

                console.log(res)

                res ? console.log(res.data) : console.log('Nothing returned')

                //console.log(this.state.profileData)
                this.setState({
                    Languages : res.data
                })
                //console.log(this.state.Languages)

            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
                //console.log(111)
            }
        })
    }




    openAdd()
    {
        this.setState({

            showAddSection : true

        })
    }

    closeAdd()
    {
        this.setState({

            showAddSection : false

        })
    }


    OpenEdit(language)
    {
        this.openAdd();

        this.setState({
            showEditSection : true,

            LanguageForEdit : language

        })
    }
     

    CloseEdit()
    {
        this.setState({
            showEditSection :false
        })
    }


    handleInput(event, {value})
    {
        this.setState({
            Name : value
        })
        console.log(this.state.Name)
    }


    handleDropDownChange(event, {value})
    {
        console.log(value)

        this.setState({
            Level : value
        })
    }


    updateLanguage()
    {
        const newLanguage = [{

            
            Name : this.state.Name,
            Level : this.state.Level 

        }]

        this.props.controlFunc(this.props.componentId, newLanguage) 
        //console.log(newLanguage)   
    }


    handleInputEdit(event, {value})
    {
        const data = Object.assign({}, this.state.LanguageForEdit)
        data.name = value

        console.log(data)

        //console.log(value)

        this.setState({
            LanguageForEdit : data
        })
    }


    handleDropDownEdit(event, {value})
    {
        const data = Object.assign({}, this.state.LanguageForEdit)
        data.level = value

        console.log(data)

        this.setState({
            LanguageForEdit : data
        })
    }

    //DeleteLanguage(userId, languageId)
    DeleteLanguage(languageId)
    {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/DeleteLanguage',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            data: JSON.stringify(languageId),
            success: function (res) {
                console.log(res)
                if (res.success == true) {
                    TalentUtil.notification.show("Profile updated sucessfully", "success", null, null)
                } else {
                    TalentUtil.notification.show("Profile did not update successfully", "error", null, null)
                }

            }.bind(this),
            error: function (res, a, b) {
                console.log(res)
                console.log(a)
                console.log(b)
            }
        })
    }


    EditLanguage()
    {
        // const data = {
        //     Name: "Testing-1", 
        //     Level: "Testing-1", 
        //     Id: "5f914e02040251469ccee31e", 
        //     CurrentUserId: null
        // }

        const data = Object.assign({}, this.state.LanguageForEdit)

        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/UpdateLanguage',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            data: JSON.stringify(data),
            success: function (res) {
                console.log(res)
                if (res.success == true) {
                    TalentUtil.notification.show("Language Edit sucessfully", "success", null, null)
                } else {
                    TalentUtil.notification.show("Language did not edit successfully", "error", null, null)
                }

            }.bind(this),
            error: function (res, a, b) {
                console.log(res)
                console.log(a)
                console.log(b)
            }
        })
    }


    renderAdd()
    {
        //console.log(this.state.LanguageForEdit)
        //console.log(this.state.LanguageForEdit.name)

        const levels = [

            {
                key : 'Basic',
                text : 'Basic',
                value : 'Basic'
            },

            {
                key : 'Conversational',
                text : 'Conversational',
                value : 'Conversational'
            },
            
            {
                key : 'Fluent',
                text : 'Fluent',
                value : 'Fluent'
            },

            {
                key : 'Native',
                text : 'Native',
                value : 'Native'
            }

        ]
 
        //console.log('Add Sec opened')
        return(
                
            this.state.showEditSection == false ? 

            <Form.Group>

                <Form.Input 
                    placeholder = 'Add Language'
                    content = {this.state.Name}
                    onChange = {this.handleInput}
                />

                <Dropdown
                    selection
                    options = {levels}
                    placeholder = 'Language Level'
                    //value = {this.state.Level}
                    text = {this.state.Level}
                    onChange = {this.handleDropDownChange}
                /> 

                <Button 
                    content = 'Add'
                    color = 'black'
                    onClick = {this.updateLanguage}
                />

                <Button 
                    content = 'Cancel'
                    onClick = {this.closeAdd}
                />


            </Form.Group>

            :

            <Form.Group>

                <Form.Input 
                    defaultValue = {this.state.LanguageForEdit.name}
                    //content = {this.state.NameForEdit}
                    onChange = {this.handleInputEdit}
                />

                <Dropdown
                    selection
                    options = {levels}
                    placeholder = 'Language Level'
                    value = {this.state.LanguageForEdit.level}
                    text = {this.state.LanguageForEdit.level}
                    onChange = {this.handleDropDownEdit}
                /> 

                <Button 
                    content = 'Save'
                    color = 'black'
                    onClick = {this.EditLanguage}
                />

                <Button 
                    content = 'Cancel'
                    onClick = {this.closeAdd}
                />


            </Form.Group>


        )
        
        

    }


    renderBoth()
    {
        return(
            <div className = 'ui sixteen wide column'>
                {this.renderAdd()}
                {this.renderTable()}
            </div>
        )

    }



    renderTable() {

        //console.log('display comp state'+this.state.showAddSection)
        const languages = this.state.Languages
        //console.log(languages)
        const id = this.props.userId

        return(

            <div className = 'ui sixteen wide column'>

                
            
                {/* {this.state.showAddSection ? this.renderAdd() : console.log()} */}
                {/* {this.renderAdd()} */}
            

            <Table>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Language</Table.HeaderCell>
                        <Table.HeaderCell>Level</Table.HeaderCell>
                        <Table.HeaderCell>
                            <Button 
                                content = 'Add New'
                                icon = 'add'
                                floated = 'right'
                                color = 'black'
                                onClick = {this.openAdd}
                            />
                        </Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>

                    {languages.map(
                        (lang, key)=>{

                            return(
                            <Table.Row key = {key}>
                                <Table.Cell>{lang.name}</Table.Cell>
                                <Table.Cell>{lang.level}</Table.Cell>
                                <Table.Cell>

                                    <Button 
                                        icon = 'cancel'
                                        floated = 'right'
                                        color = 'black'
                                        size = 'mini'
                                        onClick = {()=>{this.DeleteLanguage(lang.id)}}
                                    />
                                    <Button 
                                        icon = 'pencil'
                                        floated = 'right'
                                        color = 'black'
                                        size = 'mini'
                                        onClick = {()=>{this.OpenEdit(lang)}}
                                    />
                                    
                                </Table.Cell>
                            </Table.Row>
                            )
                        }
                    )}

                </Table.Body>


            </Table>

            </div>

        )  
    }




    render()
    {
        //console.log('render comp state'+this.state.showAddSection)

        return(
            this.state.showAddSection ? this.renderBoth() : this.renderTable()
        )
        

    }










}