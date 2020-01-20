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
                        <th>Updated</th>
                        <th>Owner</th>
                        <th>Pusher</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.dockerImages.map((dockerImage: DockerImagesStore.DockerImage) =>
                        <tr key={dockerImage.id}>
                            <td>{dockerImage.name}</td>
                            <td>{dockerImage.tag}</td>
                            <td> {new Intl.DateTimeFormat("de-DE", {
                                year: "numeric",
                                month: "2-digit",
                                hour: 'numeric',
                                minute: 'numeric',
                                second: 'numeric',
                                day: "2-digit",
                                hour12: false,
                            }).format(new Date(dockerImage.updated))}</td>
                            <td>{dockerImage.owner}</td>
                            <td>{dockerImage.pusher}</td>
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
