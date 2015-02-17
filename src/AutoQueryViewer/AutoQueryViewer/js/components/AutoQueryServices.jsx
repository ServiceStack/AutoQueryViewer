/** @jsx React.DOM */

var React = React || require('react');
var module = module || {};

var AutoQueryServices = React.createClass({
	componentDidMount: function(e) {
		var self = this;
		$.ajax({
			url: 'services',
			dataType: 'json',
			type: 'GET',
			success: function(response) {
				self.setState(response);
			}
		});
	},
	getInitialState: function () {
		return {results: []};
	},
    renderRow: function(o) {
        return (
            <tr>
                <td>{o.serviceName}</td>
                <td>{o.serviceBaseUrl}</td>
                <td>{o.serviceDescription}</td>
            </tr>
        );
    },
	render: function() {
		return (
            <table className="table table-striped">
                <thead>
                    <th>Name</th>
                    <th>Base Url</th>
                    <th>Description</th>
                </thead>
                <tbody>
                    {this.state.results.map(this.renderRow)}
                </tbody>
            </table>
		);
    }
});

module.exports = AutoQueryServices