import React, { Component } from 'react'
import { Button } from 'semantic-ui-react'
import { ChildSingleInput } from '../Form/SingleInput.jsx'

export default class TalentDescription extends Component {

    constructor(props) {
        super(props)

        const details = Object.assign({}, props.description)
    
        this.state = {
             
            showEditSection : false,
            description : details

        }

        this.renderDisplay = this.renderDisplay.bind(this)
        this.renderEdit = this.renderEdit.bind(this)
        this.openEdit = this.openEdit.bind(this)
        this.closeEdit = this.closeEdit.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.saveChanges = this.saveChanges.bind(this)
    }
    

    openEdit()
    {
        const details = Object.assign({}, this.props.description)
        this.setState({
            showEditSection : true,
            description : details
        })
        console.log(this.state.description)
    }

    closeEdit()
    {
        this.setState({
            showEditSection : false
        })
    }

    handleChange(event)
    {
        const newData = Object.assign({}, this.state.description)
        newData[event.target.name] = event.target.value
        this.setState({
            description : newData
        })
        console.log(this.state.description)
        console.log(newData)
    }


    saveChanges()
    {
        const data = Object.assign({}, this.state.description)
        this.props.controlFunc(data)
        this.closeEdit()
    }







    render() {

        //console.log(this.state.description)

        return (
            
            this.state.showEditSection ? this.renderEdit() : this.renderDisplay()

        )
    }

    renderDisplay()
    {
        let description = this.props.description.description
        let summary = this.props.description.summary

        return(
            <div className = 'row'>
                <div className = 'ui sixteen wide column'>
                    <React.Fragment>
                        <p>Description: </p><br/>
                        <p>{description}</p>

                        <p>Summary: </p><br/>
                        <p>{summary}</p>

                    </React.Fragment>
                    <Button 
                    content = 'Edit'
                    color = 'black'
                    floated = 'right'
                    onClick = {this.openEdit}
                    />
                </div>
            </div>
        )
    }


    renderEdit()
    {
        

        return(
            <div className='ui sixteen wide column'>
            
            <ChildSingleInput 
            
            label = 'Description'
            inputType = 'text'
            name = 'description'
            value = {this.state.description.description}
            placeholder = 'Please tell us about your story.'
            maxLength = {600}
            controlFunc = {this.handleChange}
            errorMessage='Please enter valid description'

            />

            <ChildSingleInput 
            
            label = 'Summary'
            inputType = 'text'
            name = 'summary'
            value = {this.state.description.summary}
            placeholder = 'Please tell us about yourself briefly'
            maxLength = {150}
            controlFunc = {this.handleChange}
            errorMessage='Please enter valid summary'

            />

            <Button 
            content = 'Save'
            color = 'black'
            onClick = {this.saveChanges}
            />
            
            <Button 
            content = 'Cancel'
            onClick = {this.closeEdit}
            />
            
            
            </div>
        )
    }













}


