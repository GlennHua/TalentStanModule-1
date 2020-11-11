/* Skill section */
import React from 'react';
import Cookies from 'js-cookie';
import {Button, Dropdown, Form, Tab, Table} from 'semantic-ui-react';

export default class Skill extends React.Component {

    constructor(props) {

        super(props);

        this.state = {

            openAddSection : false,
            openEditSection : false,

            skills : [],

            newSkill : {

                Id : '',
                UserId : '',
                IsDeleted : false,
                Skill : '',
                ExperienceLevel : ''

            },

            skillForEdit : {

                Id : '',
                UserId : '',
                IsDeleted : false,
                Skill : '',
                ExperienceLevel : ''

            }

        }


        this.renderTable = this.renderTable.bind(this)
        this.renderAdd = this.renderAdd.bind(this)
        this.openAdd = this.openAdd.bind(this)
        this.closeAdd = this.closeAdd.bind(this)
        this.renderBoth = this.renderBoth.bind(this)
        this.openEdit = this.openEdit.bind(this)
        this.getSkills = this.getSkills.bind(this)
        this.addSkill = this.addSkill.bind(this)
        this.handleAddInput = this.handleAddInput.bind(this)
        this.handleAddDropDown = this.handleAddDropDown.bind(this)
        this.handleEditInput = this.handleEditInput.bind(this)
        this.handleEditDropdown = this.handleEditDropdown.bind(this)
        this.updateSkill = this.updateSkill.bind(this)
        this.deleteSkill = this.deleteSkill.bind(this)
      
    };


    componentDidMount()
    {
        this.getSkills()
        //this.addSkill()
    }


    openAdd()
    {
        this.setState({

            openAddSection : true

        })
    }


    closeAdd()
    {
        this.setState({

            openAddSection : false

        })
    }
  

    openEdit(skill)
    {
        console.log(skill)
        const data = Object.assign({}, this.state.skillForEdit)

        data.Skill = skill.name
        data.ExperienceLevel = skill.level
        data.Id = skill.id


        this.setState({

            openEditSection : true,
            skillForEdit : data

        })
        this.openAdd()

    }

    handleAddInput(event, {value})
    {
        //console.log(value)
        const data = Object.assign({}, this.state.newSkill)
        
        data.Skill = value

        console.log(data)

        this.setState({

            newSkill : data

        })
    }

    handleAddDropDown(event, {value})
    {
        const data = Object.assign({}, this.state.newSkill)

        data.ExperienceLevel = value

        console.log(data)

        this.setState({

            newSkill : data

        })
    }


    handleEditInput(event, {value})
    {
        const data = Object.assign({}, this.state.skillForEdit)
        data.Skill = value
        console.log(data)
        this.setState({
            skillForEdit : data
        })
    }

    handleEditDropdown(event, {value})
    {

        const data = Object.assign({}, this.state.skillForEdit)
        data.ExperienceLevel = value
        console.log(data)
        this.setState({
            skillForEdit : data
        })

        //console.log(this.state.skillForEdit)
    }

    


    getSkills()
    {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/GetSkills',
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

                    skills : res.data

                })


            }.bind(this),
            error: function(res)
            {
                console.log(res.status)
            }
        })
    }


    addSkill()
    {
        const data = Object.assign({}, this.state.newSkill)

        var cookies = Cookies.get('talentAuthToken');

        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/AddSkill',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
            dataType: "json",
            success: function (res) {

                //console.log(res)

                res ? console.log(res.data) : console.log('Nothing returned')


            }.bind(this),
            error: function(res)
            {
                console.log(8787)
                console.log(res.status)
            }
        })
    }


    updateSkill()
    {
        const data = Object.assign({}, this.state.skillForEdit)

        var cookies = Cookies.get('talentAuthToken');

        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/UpdateSkill',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(data),
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


    deleteSkill(id)
    {

        var cookies = Cookies.get('talentAuthToken');

        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/DeleteSkill',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(id),
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


    



    renderAdd()
    {
        const options = [

            {
                key : 'Beginner',
                text : 'Beginner',
                value : 'Beginner'
            },

            {
                key : 'Intermediate',
                text : 'Intermediate',
                value : 'Intermediate'
            },

            {
                key : 'Expert',
                text : 'Expert',
                value : 'Expert'
            }

        ]


        return(

            this.state.openEditSection == false ?

            <Form.Group>

                <Form.Input 
                    placeholder = 'Add Skill'
                    onChange = {this.handleAddInput}
                />

                <Dropdown 
                    selection
                    options = {options}
                    placeholder = 'Skill Level'
                    onChange = {this.handleAddDropDown}
                />

                <Button 
                    color = 'black'
                    content = 'Add'
                    onClick = {this.addSkill}
                />

                <Button 
                    content = 'Cancel'
                    onClick = {this.closeAdd}
                />

            </Form.Group>

            :

            <Form.Group>

                <Form.Input 
                    defaultValue = {this.state.skillForEdit.Skill}
                    onChange = {this.handleEditInput}
                />

                <Dropdown 
                    selection
                    options = {options}
                    defaultValue = {this.state.skillForEdit.ExperienceLevel}
                    onChange = {this.handleEditDropdown}
                />

                <Button 
                    color = 'black'
                    content = 'Save'
                    onClick = {this.updateSkill}
                />

                <Button 
                    content = 'Cancel'
                    onClick = {this.closeAdd}
                />

            </Form.Group>
        )

    }



    renderTable()
    {
        const currentSkills = this.state.skills

        //console.log(currentSkills.length)

        return(

            <div className = 'ui sixteen wide column'>
            <Table>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>Skill</Table.HeaderCell>
                        <Table.HeaderCell>Level</Table.HeaderCell>
                        <Table.HeaderCell>

                            <Button 
                                color = 'black'
                                icon = 'add'
                                content = 'Add New'
                                floated = 'right'
                                onClick = {this.openAdd}
                            />

                        </Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>

                    { 

                        currentSkills.map(
                            (skill, key)=>{
                            
                            return(
                            <Table.Row key = {key}>
                                <Table.Cell>{skill.name}</Table.Cell>
                                <Table.Cell>{skill.level}</Table.Cell>
                                <Table.Cell>

                                    <Button 
                                        icon = 'cancel'
                                        floated = 'right'
                                        size = 'tiny'
                                        color = 'black'
                                        onClick = {()=>this.deleteSkill(skill.id)}
                                    />

                                    <Button 
                                        icon = 'pencil'
                                        floated = 'right'
                                        size = 'tiny'
                                        color = 'black'
                                        onClick = {()=>this.openEdit(skill)}
                                    />
                                
                                </Table.Cell>
                            </Table.Row>
                            )
                        })

                    }

                </Table.Body>

            </Table>
            </div>
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


   render() {

        return(
            this.state.openAddSection ? this.renderBoth() : this.renderTable()
        )
        
    }
}

