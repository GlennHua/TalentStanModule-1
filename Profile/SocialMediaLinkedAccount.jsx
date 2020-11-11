/* Social media JSX */
import React, {Component} from 'react';
import { ChildSingleInput } from '../Form/SingleInput.jsx';
import { Popup, Button, Label } from 'semantic-ui-react';

export default class SocialMediaLinkedAccount extends Component {
    constructor(props) {
        super(props);

        const details = this.props.linkedAccounts ? Object.assign({}, this.props.linkedAccounts) :
        {
            github: "",
            linkedIn: ""
        }

        this.state = {
            showEditSection : false,
            info : details
        }

        //console.log(this.props.linkedAccounts)
        
        this.openEdit = this.openEdit.bind(this)
        this.closeEdit = this.closeEdit.bind(this)
        this.renderEdit = this.renderEdit.bind(this)
        this.renderDisplay = this.renderDisplay.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.saveNewData = this.saveNewData.bind(this)
 
    }

    componentDidMount() {
        $('.ui.button.social-media')
            .popup();
    }


    
    openEdit()
    {
        const details = Object.assign({}, this.props.linkedAccounts)
        this.setState({
            showEditSection : true,
            info : details
        })
    }

    closeEdit()
    {
        this.setState({
            showEditSection : false
        })
    }

    handleChange(event)
    {
        const newData = Object.assign({}, this.state.info)
        newData[event.target.name] = event.target.value
        this.setState({
            info : newData
        }) 
        console.log(this.state.info)
    }

    saveNewData()
    {
        const newData = Object.assign({}, this.state.info)
        //console.log(newData)
        this.props.saveProfileData(this.props.componentId, newData)
        this.closeEdit()
    }


    render() {

        return(
            //this.render()
            //this.renderDisplay()
            this.state.showEditSection ? this.renderEdit() : this.renderDisplay()
            //this.state.showEditSection ? this.renderDisplay() : this.renderEdit()
        )
    }


    renderEdit()
    {
        //console.log('edit page open')
        //console.log(this.state.info)

        return(
            <div className='ui sixteen wide column'>

                <ChildSingleInput 
                inputType = "text"
                label = 'LinkedIn'
                name = 'linkedIn'
                value = {this.state.info.linkedIn}
                placeholder = 'Enter your LinkedIn URL'
                controlFunc = {this.handleChange}
                maxLength = {80}
                errorMessage = 'Please enter valid URL'
                />


                <ChildSingleInput
                inputType = 'text' 
                label = 'GitHub'
                placeholder = 'Enter your GitHub URL'
                name = 'github'
                value = {this.state.info.github}
                maxLength = {80}
                errorMessage = 'Please enter valid URL'
                controlFunc = {this.handleChange}
                />
                

                <span>
                <Button 
                content = 'Save'
                className="ui teal button"
                onClick={this.saveNewData}
                />

                <Button 
                content = 'Cancel'
                onClick = {this.closeEdit}
                />
                </span>

            </div>
        )

    }


    renderDisplay()
    {
        //console.log('display page open')

        return(
            <div className='ui sixteen wide column'>
                <Button
                icon = 'linkedin'
                color = 'linkedin'
                content = 'LinkedIn'
                size = 'medium'
                />

                <Button 
                icon = 'github square'
                color = 'black'
                content = 'GitHub'
                size = 'medium'
                />

                <Button 
                content = 'Edit'
                color = 'black'
                onClick = {this.openEdit}
                floated = 'right'
                />
            </div>
        )

    }

}