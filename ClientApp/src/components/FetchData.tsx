import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as DockerImagesStore from '../store/DockerImages';

// At runtime, Redux will merge together...
type DockerImageProps =
    DockerImagesStore.DockerImagesState // ... state we've requested from the Redux store
    & typeof DockerImagesStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


class FetchData extends React.PureComponent<DockerImageProps> {
    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel" className="display-4">Docker Images</h1>
                {this.renderDockerImagesTable()}
                {this.renderPagination()}
            </React.Fragment>
        );
    }

    private ensureDataFetched() {
        const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
        this.props.requestDockerImages(startDateIndex);
    }

    private renderDockerImagesTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Image</th>
                        <th>Tag</th>
                        <th>Owner</th>
                        <th>Pusher</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.dockerImages.map((forecast: DockerImagesStore.DockerImage) =>
                        <tr key={forecast.id}>
                            <td>{forecast.name}</td>
                            <td>{forecast.tag}</td>
                            <td>{forecast.owner}</td>
                            <td>{forecast.pusher}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    private renderPagination() {
        const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
        const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

        return (
            <div className="d-flex justify-content-between">
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
                {this.props.isLoading && <span>Loading...</span>}
                <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.dockerImages, // Selects which state properties are merged into the component's props
    DockerImagesStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchData as any);
