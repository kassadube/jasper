import {
  default as React,
  PropTypes
} from 'react'
import { connect } from 'react-redux'
import Envelope from '../Components/Envelope'
import {
  getVisibleMessages,
  toggleSavedMessage
} from './sentMessagesReducer'

const SentMessages = ({ messages, saveMessage }) => {
  let list = messages.map(m =>
    <li key={m.headers.id} className="message-list-item">
      <Envelope id={m.headers.id} queue="sent" saveMessage={saveMessage}/>
    </li>)
  return (
    <ul className="message-list">
      {list}
    </ul>
  )
}

SentMessages.propTypes = {
  messages: PropTypes.array.isRequired,
  saveMessage: PropTypes.func.isRequired
}

export default connect(
  (state) => {
    return {
      messages: getVisibleMessages(state.sent, state.sent.filter)
    }
  },
  (dispatch) => {
    return {
      saveMessage: (m) => {
        return dispatch(toggleSavedMessage(m))
      }
    }
  }
)(SentMessages)
