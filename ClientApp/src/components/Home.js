import React, { Component } from 'react';
import axios from 'axios';
import * as moment from 'moment';
import { Bar } from 'react-chartjs-2';
import { RollingRetention } from "./RollingRetention";

export class Home extends Component {

    constructor(props) {
        super(props);
        this.state = { users: [], loading: true, calculated: false, rollingRetentionText: "" };
    }

    componentDidMount() {
        this.populateWeatherData();
    }

    setUserRegistrationDate = (newValue, user) => {
        this.setState(prev => ({
            ...prev,
            users: prev.users.map(prevUser => {
                if (prevUser.userId === user.userId) {
                    return {
                        ...prevUser,
                        registrationDate: newValue
                    }
                }
                return prevUser
            })
        }))
    }

    setUserLastActivityDate = (newValue, user) => {
        this.setState(prev => ({
            ...prev,
            users: prev.users.map(prevUser => {
                if (prevUser.userId === user.userId) {
                    return {
                        ...prevUser,
                        lastActivityDate: newValue
                    }
                }
                return prevUser
            })
        }))
    }

    renderForecastsTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>UserID</th>
                        <th>Date Registration</th>
                        <th>Date Last Activity</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.users.map(forecast =>
                        <tr key={forecast.userId}>
                            <td>{forecast.userId}</td>
                            <td>{<input
                                type="text"
                                value={forecast.registrationDate}
                                onChange={(e) => this.setUserRegistrationDate(e.target.value, forecast)}
                            />}</td>
                            <td>{<input
                                type="text"
                                value={forecast.lastActivityDate}
                                onChange={(e) => this.setUserLastActivityDate(e.target.value, forecast)}
                            />}</td>
                        </tr>
                    )}
                </tbody>
            </table>

        );
    }
    validation = (validator) => {
        return this.state.users
            .map(
                (x) =>
                    validator.test(x.registrationDate) &&
                    validator.test(x.lastActivityDate)
            )
            .findIndex((x) => !x) == -1;
    };


    handleClickSave = () => {
        this.setState({ calculated: false })
        //const validator = new RegExp(/^(0?[1-9]|[12][0-9]|3[01])[.](0?[1-9]|1[012])[.]\d{4}$/);
        const validator = new RegExp(/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/);
        console.log(this.state.users);
        this.setState({ rollingRetentionText: "" });
        var t = this.validation(validator);
        if (this.validation(validator)) {
            const temp = this.state.users.map(x => {
                return {
                    ...x,
                    registrationDate: x.registrationDate, //new Date(x.registrationDate.split('.').reverse().join('.')),//new Date(`${x.registrationDate.getFullYear()}.${x.registrationDate.getMonth()}.${x.registrationDate.getDate()}`),
                    lastActivityDate: x.lastActivityDate, //new Date(x.lastActivityDate.split('.').reverse().join('.')), //new Date(x.lastActivityDate),
                }
            })
            const payload = this.state.users;
            axios
                .post('/api/users/userssave', temp)
                .then(response => console.log(response))
                .catch(error => console.log(error))
        } else {
            alert("Invalid date")
        }
    }

    rollingRetention7Day = () => {
        axios
            .get('/api/users/userscalculate')
            .then(response => {
                console.log(response);
                this.setState({ calculated: true, rollingRetentionText: `Rolling Retention 7 day = ${response.data}%` });
            })
            .catch(error => console.log(error))
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderForecastsTable();
        return (
            <div>
                {contents}
                <button className="btn btn-primary" onClick={this.handleClickSave}>Save</button>
                <button className="btn btn-primary" onClick={() => {
                    this.rollingRetention7Day()
                }}>Calculate</button>
                <p>{this.state.rollingRetentionText}</p>
                <RollingRetention visible={this.state.calculated} />

            </div>
        );
    }



    toFormatedDate = (date) => {
        let temp = new Date(date);
        let day = temp.getDate() > 9 ? temp.getDate() : '0' + temp.getDate();
        let month = temp.getMonth() + 1 > 9 ? temp.getMonth() + 1 : '0' + (temp.getMonth() + 1);
        return [day, month, temp.getFullYear()].join('.')
    }

    async populateWeatherData() {
        const response = await fetch('api/users/users');
        const data = await response.json();
        const temp = data.map(x => {
            return {
                ...x,
                registrationDate: this.toFormatedDate(x.registrationDate),
                lastActivityDate: this.toFormatedDate(x.lastActivityDate),
            }
        })

        this.setState({ users: temp, loading: false });
    }
}