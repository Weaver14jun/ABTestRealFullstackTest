import React, { useEffect, useState } from "react";
import axios from "axios";
import { Bar } from 'react-chartjs-2';

export const RollingRetention = ({ visible }) => {
    const [chartData, setChartData] = useState(null)

    useEffect(() => {
        if (visible) {
            getData()
        } else {
            setChartData(null)
        }
    }, [visible])

    const getData = () => {
        axios
            .get("/api/users/usersalive")
            .then((response) => {
                const data = response.data;
                setChartData({
                    labels: data.map(x => x.userId),
                    datasets: [{
                        label: 'My First dataset',
                        backgroundColor: 'rgb(255, 99, 132)',
                        borderColor: 'rgb(255, 99, 132)',
                        data: data.map(x => x.daysAlive),
                    }]
                })
            })
            .catch((error) => {
                console.log(error)
                alert("Chart error!")
            })
    }


    if (!visible || !chartData) return null;

    return (
        <div>
            <h2>Chart</h2>
            <Bar data={chartData} />
        </div>
    )
}