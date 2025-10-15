import { useState, useEffect } from 'react';
import { useAuth } from '@/context/auth-context';
import { api, User, Message, Conversation } from '@/lib/api';
import { encrypt, decrypt } from '@/lib/crypto';
import { Button, Input, Avatar, AvatarFallback, ScrollArea, Separator } from '@/components/ui';
import { LogOut, Send } from 'lucide-react';
import { toast } from '@/components/ui/use-toast';

const ChatPage = () => {
  const { user, logout, privateKey } = useAuth();
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    loadConversations();
    loadUsers();
  }, []);

  useEffect(() => {
    if (selectedUser) {
      loadConversation(selectedUser.id);
    }
  }, [selectedUser]);

  const loadConversations = async () => {
    try {
      const convs = await api.messages.getConversations();
      setConversations(convs);
    } catch (error) {
      console.error('Failed to load conversations', error);
    }
  };

  const loadUsers = async () => {
    try {
      const allUsers = await api.auth.getAllUsers();
      setUsers(allUsers);
    } catch (error) {
      console.error('Failed to load users', error);
    }
  };

  const loadConversation = async (userId: number) => {
    try {
      const msgs = await api.messages.getConversation(userId);
      setMessages(msgs);
    } catch (error) {
      console.error('Failed to load conversation', error);
    }
  };

  const sendMessage = async () => {
    if (!newMessage.trim() || !selectedUser || !selectedUser.publicKey) {
      toast({
        title: 'Error',
        description: 'Cannot send message. User may not have a public key.',
        variant: 'destructive',
      });
      return;
    }

    setIsLoading(true);
    try {
      const encrypted = await encrypt(newMessage, selectedUser.publicKey);
      await api.messages.sendMessage(selectedUser.id, encrypted);
      setNewMessage('');
      await loadConversation(selectedUser.id);
      await loadConversations();
    } catch (error) {
      toast({
        title: 'Error',
        description: 'Failed to send message',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };

  const decryptMessage = async (msg: Message) => {
    if (!privateKey) return msg.encryptedContent;
    try {
      return await decrypt(msg.encryptedContent, privateKey);
    } catch (error) {
      return '[Failed to decrypt]';
    }
  };

  return (
    <div className="h-screen flex">
      <div className="w-80 border-r flex flex-col">
        <div className="p-4 border-b flex items-center justify-between">
          <div>
            <h2 className="font-semibold">{user?.username}</h2>
            <p className="text-sm text-muted-foreground">{user?.email}</p>
          </div>
          <Button variant="ghost" size="icon" onClick={logout}>
            <LogOut className="h-5 w-5" />
          </Button>
        </div>

        <ScrollArea className="flex-1">
          <div className="p-2">
            <h3 className="font-semibold px-2 py-1 text-sm">All Users</h3>
            {users.map((u) => (
              <button
                key={u.id}
                onClick={() => setSelectedUser(u)}
                className={`w-full p-3 flex items-center gap-3 hover:bg-accent rounded-lg transition ${
                  selectedUser?.id === u.id ? 'bg-accent' : ''
                }`}
              >
                <Avatar>
                  <AvatarFallback>{u.username[0].toUpperCase()}</AvatarFallback>
                </Avatar>
                <div className="flex-1 text-left">
                  <p className="font-medium text-sm">{u.username}</p>
                  <p className="text-xs text-muted-foreground">{u.email}</p>
                </div>
              </button>
            ))}
          </div>
        </ScrollArea>
      </div>

      <div className="flex-1 flex flex-col">
        {selectedUser ? (
          <>
            <div className="p-4 border-b">
              <h2 className="font-semibold">{selectedUser.username}</h2>
              <p className="text-sm text-muted-foreground">
                {selectedUser.publicKey ? 'Encrypted' : 'No public key'}
              </p>
            </div>

            <ScrollArea className="flex-1 p-4">
              <div className="space-y-4">
                {messages.map((msg) => (
                  <MessageBubble
                    key={msg.id}
                    message={msg}
                    isOwn={msg.senderId === user?.id}
                    decryptMessage={decryptMessage}
                  />
                ))}
              </div>
            </ScrollArea>

            <div className="p-4 border-t">
              <div className="flex gap-2">
                <Input
                  value={newMessage}
                  onChange={(e) => setNewMessage(e.target.value)}
                  onKeyPress={(e) => e.key === 'Enter' && sendMessage()}
                  placeholder="Type a message..."
                  disabled={isLoading}
                />
                <Button onClick={sendMessage} disabled={isLoading}>
                  <Send className="h-4 w-4" />
                </Button>
              </div>
            </div>
          </>
        ) : (
          <div className="flex-1 flex items-center justify-center text-muted-foreground">
            Select a user to start messaging
          </div>
        )}
      </div>
    </div>
  );
};

const MessageBubble = ({
  message,
  isOwn,
  decryptMessage,
}: {
  message: Message;
  isOwn: boolean;
  decryptMessage: (msg: Message) => Promise<string>;
}) => {
  const [decrypted, setDecrypted] = useState('');

  useEffect(() => {
    decryptMessage(message).then(setDecrypted);
  }, [message]);

  return (
    <div className={`flex ${isOwn ? 'justify-end' : 'justify-start'}`}>
      <div
        className={`max-w-[70%] rounded-lg p-3 ${
          isOwn ? 'bg-primary text-primary-foreground' : 'bg-muted'
        }`}
      >
        <p className="text-sm">{decrypted || 'Decrypting...'}</p>
        <p className="text-xs opacity-70 mt-1">
          {new Date(message.sentAt).toLocaleTimeString()}
        </p>
      </div>
    </div>
  );
};

export default ChatPage;
